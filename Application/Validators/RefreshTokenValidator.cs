using Application.Exceptions;
using Domain.Identity;

namespace Application.Validators
{
    public static class RefreshTokenValidator
    {
        public static void Validate(RefreshToken? token)
        {
            if (token is null)
                throw ApiException.Unauthorized("الجلسة غير صالحة. برجاء تسجيل الدخول مرة أخرى.");

            if (token.RevokedAt is not null)
                throw ApiException.Unauthorized("تم استخدام الجلسة هذه مسبقًا. برجاء تسجيل الدخول مرة أخرى.");

            if (token.ExpiresAt <= DateTime.UtcNow)
                throw ApiException.Unauthorized("انتهت صلاحية الجلسة. برجاء تسجيل الدخول مرة أخرى.");
        }
    }
}
