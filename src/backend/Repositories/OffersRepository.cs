using backend.Models;
using System.Collections.Generic;
using System;

namespace backend.Repositories
{
    public static class OffersRepository
    {
        private static List<Offer> Offers {  get; set; }

        static OffersRepository()
        {
            Offers = new List<Offer>();
        }

        public static void SaveOffer(Offer offer)
        {
            Offers.Add(offer);
        }

        public static Offer GetStandardOffer()
        {
            return new Offer(999999, 999999, 999999, 999999, DateTime.Now, DateTime.Now);
        }

        public static Offer GetOfferById(int id)
        {
            return Offers.Find((Offer offer) => offer.Id == id);
        }
        public static List<Offer> GetAllOffers()
        {
            return Offers;
        }
    }
}
