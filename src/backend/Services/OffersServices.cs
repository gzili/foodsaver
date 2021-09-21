using backend.Models;
using backend.Repositories;
using System;
using System.Collections.Generic;

namespace backend.Services
{
    public static class OffersServices
    {
        public static void SaveOffer(Offer offer)
        {
            OffersRepository.SaveOffer(offer);
        }

        public static Offer GetStandardOffer()
        {
            return OffersRepository.GetStandardOffer();
        }

        public static Offer GetSpecificOffer(int id)
        {
            return OffersRepository.GetSpecificOffer(id);
        }
        public static List<Offer> GetAllOffers()
        {
            return OffersRepository.GetAllOffers();
        }
        public static Offer GenerateRandomOfffer()
        {
            Random rng = new Random();
            return new Offer(rng.Next(1, 1000), rng.Next(1, 1000), rng.Next(1, rng.Next(1, rng.Next(1, 50))), rng.Next(1, 1000), DateTime.Now, DateTime.Now);
        }
    }
}
