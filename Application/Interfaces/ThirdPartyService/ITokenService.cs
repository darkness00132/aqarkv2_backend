using Application.DTOs.User;
using Domain.Entities.UsersEnities;

namespace Application.Interfaces.ThirdPartyService
{
    public interface ITokenService
    {
        public string CreateJwtToken(User user, IList<string> roles);
    }
}
