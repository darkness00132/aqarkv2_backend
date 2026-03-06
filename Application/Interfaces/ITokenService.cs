using Application.DTOs.User;
using Infrastructure.Identity;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        public string CreateJwtToken(User user, IList<string> roles);
    }
}
