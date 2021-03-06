using backend.DTO;
using backend.DTO.Offer;
using backend.Models;

namespace backend.Services
{
    public interface IOffersService
    {
        void Create(Offer offer);
        Offer FindById(int id);
        PaginatedList<Offer> FindAllPaginated(bool includeExpired, int page, int limit, int userId);
        void Update(Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto);
        void Delete(Offer offer);
    }
}