using Application.DTOs.Auth;
using Application.DTOs.User;
using Microsoft.AspNetCore.Authentication;

namespace Application.Interfaces
{
   public interface IAuthService
    {
        Task<GoogleCallbackResult> HandleGoogleCallbackAsync(AuthenticateResult googleResult);

        Task HandleCompleteProfileAsync(CompleteProfileDTO userDTO,string? userId, CancellationToken ct = default);

        Task RegisterAsync(RegisterDTO userDTO, CancellationToken ct = default);

        Task ConfirmEmailAsync(string userId, string token, CancellationToken ct = default);

        Task<LoginResponse> LoginAsync(LoginDTO userDTO, string ip, CancellationToken ct = default);

        Task<string> ForgetPassword(string email, CancellationToken ct = default);

        Task ResetPassword(ResetPasswordDTO resetPasswordDTO);

        Task<LoginResponse> RefreshAsync(string rawRefreshToken, string ip, CancellationToken ct = default);

        Task RevokeCurrentAsync(string rawRefreshToken, CancellationToken ct = default);

        Task RevokeAllAsync(string userId, CancellationToken ct = default);
    }

}
