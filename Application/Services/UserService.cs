
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.UsersEnities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetMe(string UserId)
        {
            var user = await _userRepo.GetByIdWithBrokerProfileAsync(Guid.Parse(UserId));
            if (user is null) throw ApiException.Unauthorized();
            return _mapper.Map<UserDTO>(user);
        }
    }
}
