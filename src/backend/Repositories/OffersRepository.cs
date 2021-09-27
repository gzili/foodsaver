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

        public static void Save(Offer offer)
        {
            Offers.Add(offer);
        }

        public static Offer GetStandard()
        {
            return new Offer(999999, FoodRepository.Get(), 999999, 999999, DateTime.Now, DateTime.Now);
        }

        public static Offer GetById(int id)
        {
            return Offers.Find((Offer offer) => offer.Id == id);
        }
        public static List<Offer> GetAll()
        {
            return Offers;
        }
    }
}
