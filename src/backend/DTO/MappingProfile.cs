using AutoMapper;
using backend.DTO.Address;
using backend.DTO.Offer;
using backend.DTO.Reservation;
using backend.DTO.User;

namespace backend.DTO
{
    public class MappingProfile : Profile // it is actually not unused
    {
        public MappingProfile()
        {
            CreateMap<Models.User, UserDto>();
            CreateMap<Models.User, CreateUserDto>()
                .ReverseMap();
            CreateMap<Models.User, GiverDto>();
            
            CreateMap<Models.Address, AddressDto>();
            CreateMap<Models.Address, AddressDto>()
                .ReverseMap();

            CreateMap<Models.Offer, OfferDto>();
            CreateMap<Models.Offer, CreateOfferDto>()
                .ReverseMap();

            CreateMap<Models.Reservation, CreateReservationDto>();
            CreateMap<Models.Reservation, ReservationDto>();
        }
    }
}