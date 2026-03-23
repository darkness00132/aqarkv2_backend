using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.UsersEnities;

namespace Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<RegisterDTO, User>().ForMember(dest=>dest.UserName,opt=>opt.MapFrom(src=>src.Email));

            CreateMap<UserDTO, User>().ReverseMap();

            CreateMap<PublicUser, User>().ReverseMap();

            CreateMap<Broker, User>().ReverseMap();

            CreateMap<BrokerItemList, User>().ReverseMap();
        }
    }
}
