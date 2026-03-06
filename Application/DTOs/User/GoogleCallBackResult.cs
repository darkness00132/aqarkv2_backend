using Microsoft.AspNetCore.Http;

namespace Application.DTOs.User
{
    public class GoogleCallbackResult
    {
        public bool Success { get; set; }
        public string RedirectUrl { get; set; }
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public CookieOptions CookieOptions { get; set; }

        public static GoogleCallbackResult Failed(string redirectUrl)
        {
            return new GoogleCallbackResult
            {
                Success = false,
                RedirectUrl = redirectUrl
            };
        }


        public static GoogleCallbackResult Successed(
            string accessToken,
            string refreshToken,
            DateTime expiresAt,
            string redirectUrl)
        {
            return new GoogleCallbackResult
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RedirectUrl = redirectUrl,
                CookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = expiresAt,
                }
            };

        }
    }
}
