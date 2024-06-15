using AutoMapper;
using HotelListing.API.Core.Models.Dto.Country;
using HotelListing.API.Core.Models.Dto.Hotel;
using HotelListing.API.Core.Models.Dto.User;
using HotelListing.API.Data;

namespace HotelListing.API.Core.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();


            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, GetHotelDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();

            CreateMap<ApiUser, ApiUserDto>().ReverseMap();
            CreateMap<ApiUser, LoginUserDto>().ReverseMap();
            CreateMap<ApiUser, RegisterUserDto>().ReverseMap();
            CreateMap<ApiUser, GetUserDto>().ReverseMap();
            CreateMap<ApiUser, UpdateUserDto>().ReverseMap();
            //CreateMap<ApiUser, LoginUserDto>().ReverseMap();
        }
    }
}
