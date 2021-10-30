using AutoMapper;
using backend.DTO.Address;
using backend.DTO.Users;
using backend.Models;

namespace backend.DTO
{
    public class MappingProfile : Profile // it is actually not unused
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, CreateUserDto>()
                .ReverseMap();
            
            CreateMap<Models.Address, AddressDto>(); // wtf
            CreateMap<Models.Address, AddressDto>()
                .ReverseMap();
        }
    }
}