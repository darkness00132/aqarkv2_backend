using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;

        public AuthController(IAuthService authService, IHostEnvironment env, IConfiguration config)
        {
            _authService = authService;
            _env = env;
            _config = config;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin() 
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallBack), "Auth")
            };

            return Challenge(props,"Google");
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallBack() 
        {
                var result = await HttpContext.AuthenticateAsync("Google");

                var response = await _authService.HandleGoogleCallbackAsync(result);

                // Set refresh cookie
                if (response.RefreshToken != null)
                {
                    var options = GetCookiesOptions();
                    Response.Cookies.Append("refreshToken", response.RefreshToken, options);
                }

                return Redirect(response.RedirectUrl);
            }

        [Authorize]
        [HttpPost("completeProfile")]
        public async Task<IActionResult> CompleteProfile(CompleteProfileDTO user , CancellationToken ct) 
        {
            //get user id from access token
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _authService.HandleCompleteProfileAsync(user, userId, ct);
            return Ok();
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody]LoginDTO userDTO, CancellationToken ct)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknow";
            LoginResponse response = await _authService.LoginAsync(userDTO, ip, ct);

            var cookieOptions = GetCookiesOptions();
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

            return Ok(new { accessToken=response.AccessToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDTO user, CancellationToken ct)
        {
            await _authService.RegisterAsync(user, ct);
            return NoContent();
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token, CancellationToken ct)
        {
            await _authService.ConfirmEmailAsync(userId, token, ct);
            return Ok();
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody]string email, CancellationToken ct)
        {
            string token = await _authService.ForgetPassword(email,ct);
            return Ok(new { forgetPasswordToken=token });
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            await _authService.ResetPassword(resetPasswordDTO);
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                throw ApiException.Unauthorized("refreshToken is not readed");

            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknow";
            LoginResponse response = await _authService.RefreshAsync(refreshToken, ip, ct);

            var cookieOptions = GetCookiesOptions();
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);

            return Ok(new { accessToken = response.AccessToken });
        }

        [Authorize]
        [HttpDelete("revokeCurrent")]
        public async Task<IActionResult> RevokeCurrentToken(CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return Unauthorized();

            await _authService.RevokeCurrentAsync(refreshToken, ct);
            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }

        [Authorize]
        [HttpDelete("revokeAll")]
        public async Task<IActionResult> RevokeAllToken(string userId, CancellationToken ct)
        {
            await _authService.RevokeAllAsync(userId, ct);
            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }

        private CookieOptions GetCookiesOptions(bool sameSite=true) 
        {
            var expiresAt = DateTimeOffset.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenLifetimeDays"));

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = _env.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Strict,
                Expires = expiresAt,
            };
            return cookieOptions;
        }
    }
}
