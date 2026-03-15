using Application.DTOs.User;
using Domain.Identity;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        public string CreateJwtToken(User user, IList<string> roles);
    }
}
