using backend.DTO.Offer;
using backend.Models;

namespace backend.Repositories
{
    public interface IOffersRepository : IRepositoryBase<Offer>
    {
        void Update(Offer offer, UpdateOfferDto updateOfferDto, FoodDto foodDto);
        void Delete(Offer offer);
    }
}