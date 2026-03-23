using Application.DTOs.Auth;
using Application.DTOs.User;
using Microsoft.AspNetCore.Authentication;

namespace Application.Interfaces
{
   public interface IAuthService
    {
        Task<GoogleCallbackResult> HandleGoogleCallbackAsync(AuthenticateResult googleResult);

        Task HandleCompleteProfileAsync(CompleteProfileDTO userDTO,string? userId);

        Task RegisterAsync(RegisterDTO userDTO);

        Task ConfirmEmailAsync(string userId, string token);

        Task<LoginResponse> LoginAsync(LoginDTO userDTO, string ip);

        Task<string> ForgetPassword(string email);

        Task ResetPassword(ResetPasswordDTO resetPasswordDTO);

        Task<LoginResponse> RefreshAsync(string rawRefreshToken, string ip);

        Task RevokeCurrentAsync(string rawRefreshToken);

        Task RevokeAllAsync(string userId);
    }

}
