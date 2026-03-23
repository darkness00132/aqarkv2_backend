using Application.DTOs.User;
using Domain.Entities.UsersEnities;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        public string CreateJwtToken(User user, IList<string> roles);
    }
}
