
using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetMe(string UserId);
    }
}
