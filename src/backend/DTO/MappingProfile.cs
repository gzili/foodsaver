using AutoMapper;
using backend.DTO.Address;
using backend.DTO.Offers;
using backend.DTO.Reservation;
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
            CreateMap<User, GiverDto>();
            
            CreateMap<Models.Address, AddressDto>(); // wtf
            CreateMap<Models.Address, AddressDto>()
                .ReverseMap();

            CreateMap<Offer, OfferDto>();
            CreateMap<Offer, CreateOfferDto>()
                .ReverseMap();

            CreateMap<Models.Reservation, ReservationDto>();
        }
    }
}