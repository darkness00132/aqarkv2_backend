using Application.DTOs.Ad;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mapping
{
    public class AdMapping : Profile
    {
        public AdMapping()
        {
            CreateMap<Ad, AdDTO>()
            .ForMember(dest => dest.AdType, opt => opt.MapFrom(src => src.AdType.ToArabic()))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.ToArabic()));

            CreateMap<UpdateAdDTO, Ad>();

            CreateMap<CreateAdDTO, Ad>().ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Ad, AdListItemDTO>()
                .ForMember(dest => dest.CoverImage, opt => opt.MapFrom(src => src.Images.FirstOrDefault()))
                .ForMember(dest => dest.AdType, opt => opt.MapFrom(src => src.AdType.ToArabic()))
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.ToArabic()));
        }
    }
}
