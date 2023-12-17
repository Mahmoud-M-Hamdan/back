using Api.DTO;
using Api.Extensions;
using Api.Model;
using AutoMapper;

namespace Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {


            CreateMap<UserModel, UserDto>().ForMember(dest => dest.PhotoUrl, op => op.MapFrom(src => src.Photos.SingleOrDefault(photo => photo.IsMain == true).Url)).ForMember(dest => dest.Photos, op => op.MapFrom(src => src.Photos))
            .ForMember(dest => dest.Age, op => op.MapFrom(src => src.Birthday.GetAgeFromBirth()));
            CreateMap<UserDto, UserModel>();
            CreateMap<Photo, PhotoDto>();

        }
    }
}