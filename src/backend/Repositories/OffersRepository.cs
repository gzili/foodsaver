using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public class OffersRepository : IRepository<Offer>
    {
        private readonly AppDbContext _appDbContext;
        public Dictionary<int, IEnumerable<Offer>> OffersByUser { get; private set; }
        
        public OffersRepository()
        {
            _appDbContext = AppDbContext.GetObject();
            GroupOffersByUser();
        }

        private void GroupOffersByUser()
        {
            var byUser = _appDbContext.DbLists.Users.GroupJoin(
                _appDbContext.DbLists.Offers,
                user => user,
                offer => offer.Giver,
                (user, offerCollection) =>
                    new
                    {
                        UserId = user.Id,
                        Offers = offerCollection
                    }).ToDictionary(o => o.UserId, o => o.Offers);
            OffersByUser = byUser;
        }

        public void Save(Offer newOffer)
        {
            _appDbContext.DbLists.Offers.Add(newOffer);
            GroupOffersByUser();
        }

        public Offer GetById(int id)
        {
            return _appDbContext.DbLists.Offers.Find(x => x.Id == id);
        }

        public List<Offer> GetAll()
        {
            return _appDbContext.DbLists.Offers;
        }
    }
}