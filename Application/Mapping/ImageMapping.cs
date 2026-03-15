using Application.DTOs.Image;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class ImageMapping:Profile
    {
        public ImageMapping()
        {
            CreateMap<Image, ImageDTO>().ReverseMap();
        }
    }
}
