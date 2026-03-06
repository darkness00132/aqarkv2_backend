using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService, IEmailService emailService, IMapper mapper, IRefreshTokenRepo refreshTokenRepo, IUnitOfWork uow, IConfiguration config)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _mapper = mapper;
            _refreshTokenRepo = refreshTokenRepo;
            _uow = uow;
            _config = config;
        }

        public async Task ConfirmEmailAsync(string userId, string token, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw ApiException.NotFound("هذا المستخدم غير موجود.");


            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                throw ApiException.BadRequest("رابط تأكيد البريد الإلكتروني غير صالح أو تم نسخه بشكل خاطئ.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded) throw ApiException.BadRequest("تعذر تأكيد البريد الإلكتروني. قد يكون الرابط منتهيًا أو غير صالح.");
        }

        public async Task<string> ForgetPassword(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw ApiException.NotFound("لا يوجد حساب بهذا البريد الإلكتروني.");

            bool isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isConfirmed) throw ApiException.Forbidden("لا يمكن إعادة تعيين كلمة المرور قبل تأكيد البريد الإلكتروني.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string FrontendUrl = _config.GetValue<string>("FrontendUrl")!;

            string url = $"{FrontendUrl}/resetPassword?email={Uri.EscapeDataString(email)}&token={encodedToken}";

            return url;
        }

        public async Task<GoogleCallbackResult> HandleGoogleCallbackAsync(AuthenticateResult googleResult)
        {
            string frontendUrl = _config.GetValue<string>("frontendUrl");

            if (!googleResult.Succeeded)
            {
                return GoogleCallbackResult.Failed($"{frontendUrl}/login?error=google_failed");
            }

            string? email = googleResult.Principal.FindFirstValue(ClaimTypes.Email);
            string? name = googleResult.Principal.FindFirstValue(ClaimTypes.Name);

            if (email == null)
            {
                return GoogleCallbackResult.Failed($"{frontendUrl}/login?error=email_missing");
            }

            // find or create user
            User? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                //create new user
                user = new User
                {
                    Name = name,
                    UserName = email,
                    Email = email,
                    IsProfileCompleted = false
                };

                await _userManager.CreateAsync(user);
            }

            string accessToken = await CreateAccessToken(user);

            string refreshToken = await GenerateRefreshToken(user, ip:"Unknown");
            int ExpiresAt = _config.GetValue<int>("Jwt:RefreshTokenLifetimeDays");

            return GoogleCallbackResult.Successed(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddDays(ExpiresAt),
                user.IsProfileCompleted ?
                    $"{frontendUrl}/auth/success?accessToken={accessToken}" :
                    $"{frontendUrl}/auth/completeProfile?accessToken={accessToken}"
            );

        }

        public async Task<LoginResponse> LoginAsync(LoginDTO userDTO, string ip, CancellationToken ct = default)
        {
            User? user = await _userManager.FindByEmailAsync(userDTO.Email);

            if (user is null) throw ApiException.Unauthorized("بيانات الدخول غير صحيحة.");
            if (!user.EmailConfirmed) throw ApiException.Forbidden("لم يتم تأكيد البريد الإلكتروني بعد. برجاء مراجعة بريدك وتأكيد الحساب.");

            bool passwordValid = await _userManager.CheckPasswordAsync(user, userDTO.Password);
            if (!passwordValid) throw ApiException.Unauthorized("بيانات الدخول غير صحيحة.");

            string accessToken = await CreateAccessToken(user);

            // Generate refresh token
            string rawRefresh = await GenerateRefreshToken(user, ip);
            await _uow.SaveChangesAsync();

            return new LoginResponse { AccessToken = accessToken, RefreshToken = rawRefresh };
        }

        public async Task<LoginResponse> RefreshAsync(string rawRefreshToken, string ip, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(rawRefreshToken))
                throw ApiException.Unauthorized("انتهت الجلسة. برجاء تسجيل الدخول مرة أخرى.");

            string hashToken = Hash(rawRefreshToken);

            await using var tx = await _uow.BeginTransactionAsync(ct);

            RefreshToken? stored = await _refreshTokenRepo.GetByHashAsync(hashToken, ct);

            RefreshTokenValidator.Validate(stored);

            // Use a single timestamp for consistency
            var utcNow = DateTime.UtcNow;

            //Revoke old token
            stored!.RevokedAt = utcNow;

            //Create new Refresh Token
            string newRefreshToken = await GenerateRefreshToken(stored.User, ip);

            await _uow.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            if (stored.User is null) throw ApiException.Unauthorized("الجلسة غير صالحة. برجاء تسجيل الدخول مرة أخرى.");

            var access = await CreateAccessToken(stored.User);

            return new LoginResponse { RefreshToken = newRefreshToken, AccessToken = access };
        }

        public async Task RegisterAsync(RegisterDTO userDTO, CancellationToken ct = default)
        {
            var user = _mapper.Map<User>(userDTO);
            user.IsProfileCompleted = true;

            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                var msg = string.Join(" | ", result.Errors.Select(e => e.Description));
                throw ApiException.Conflict(msg);
            }

            string role = UserRoles.User.ToString();
            await _userManager.AddToRoleAsync(user, role);


            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            await _emailService.SendValidationEmailAsync("delivered@resend.dev", encodedToken, user.Id);
        }

        public async Task ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            User? user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user is null) throw ApiException.NotFound("لا يوجد حساب بهذه البيانات.");

            string decodedToken;
            try
            {
                var tokenBytes = WebEncoders.Base64UrlDecode(resetPasswordDTO.Token);
                decodedToken = Encoding.UTF8.GetString(tokenBytes);
            }
            catch
            {
                throw ApiException.BadRequest("رابط إعادة تعيين كلمة المرور غير صالح أو تم نسخه بشكل خاطئ.");
            }

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDTO.NewPassword);

            if (!result.Succeeded) throw ApiException.BadRequest(string.Join(" | ", result.Errors.Select(e => e.Description)));

            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task RevokeAllAsync(string userId, CancellationToken ct = default)
        {
            if (!Guid.TryParse(userId, out var uid))
                throw ApiException.BadRequest("معرّف المستخدم غير صحيح.");

            var tokens = await _refreshTokenRepo.GetActiveByUserAsync(uid, ct);
            if (tokens.Count == 0) return;

            var now = DateTime.UtcNow;
            foreach (var t in tokens) t.RevokedAt = now;

            await _uow.SaveChangesAsync(ct);
        }

        public async Task RevokeCurrentAsync(string rawRefreshToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(rawRefreshToken))
                return;

            var hash = Hash(rawRefreshToken);
            var stored = await _refreshTokenRepo.GetByHashAsync(hash, ct);
            if (stored is null) return;

            stored.RevokedAt = DateTime.UtcNow;
            await _uow.SaveChangesAsync(ct);
        }

        private string Hash(string raw)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(raw)));

        }

        private async Task<string> CreateAccessToken(User user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            string accessToken = _tokenService.CreateJwtToken(user, roles);
            return accessToken;
        }

        private async Task<string> GenerateRefreshToken(User user, string ip)
        {
            string rawRefresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            string hash = Hash(rawRefresh);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IpAddress = ip
            };
            await _refreshTokenRepo.AddAsync(refreshToken);

            return rawRefresh;
        }
    }
}
