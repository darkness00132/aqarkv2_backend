using Application.DTOs.User;
using AutoMapper;
using Infrastructure.Identity;

namespace Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<RegisterDTO, User>().ForMember(dest=>dest.UserName,opt=>opt.MapFrom(src=>src.Email));

            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
