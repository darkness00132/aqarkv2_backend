using Application.DTOs.User;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
                Response.Cookies.Append("refreshToken", response.RefreshToken, response.CookieOptions);
            }

            return Redirect(response.RedirectUrl);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody]LoginDTO userDTO, CancellationToken ct)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknow";
            LoginResponse response = await _authService.LoginAsync(userDTO, ip, ct);

            var expiresAt = DateTimeOffset.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenLifetimeDays"));
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = !_env.IsDevelopment(),
                SameSite = _env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
                Expires = expiresAt,
            };
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
        public async Task<IActionResult> ForgetPassword([FromBody]string email)
        {
            string token = await _authService.ForgetPassword(email);
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
                return Unauthorized();

            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknow";
            LoginResponse response = await _authService.RefreshAsync(refreshToken, ip, ct);

            var expiresAt = DateTimeOffset.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenLifetimeDays"));
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = !_env.IsDevelopment(),
                SameSite = _env.IsDevelopment()?SameSiteMode.Lax:SameSiteMode.Strict,
                Expires = expiresAt,
            };

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
        public async Task<IActionResult> RevokeAllToken([FromBody]string userId, CancellationToken ct)
        {
            await _authService.RevokeAllAsync(userId, ct);
            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }
    }
}
