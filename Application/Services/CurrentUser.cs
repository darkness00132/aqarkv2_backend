using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUser(IHttpContextAccessor accessor)
        { _accessor = accessor; }

        public bool IsAuthenticated =>
            _accessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public string? UserId =>
            _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? _accessor.HttpContext?.User?.FindFirstValue("sub"); // common JWT user id
    }
}
