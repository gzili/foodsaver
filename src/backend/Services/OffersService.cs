using backend.Models;
using backend.Repositories;
using System;
using System.Collections.Generic;

namespace backend.Services
{
    public static class OffersService
    {
        public static void SaveOffer(Offer offer)
        {
            OffersRepository.SaveOffer(offer);
        }

        public static Offer GetStandardOffer()
        {
            return OffersRepository.GetStandardOffer();
        }

        public static Offer GetOfferById(int id)
        {
            return OffersRepository.GetOfferById(id);
        }
        public static List<Offer> GetAllOffers()
        {
            return OffersRepository.GetAllOffers();
        }
        public static Offer GenerateRandomOffer()
        {
            Random rng = new Random();
            return new Offer(rng.Next(1, 1000), rng.Next(1, 1000), rng.Next(1, rng.Next(1, rng.Next(1, 50))), rng.Next(1, 1000), DateTime.Now, DateTime.Now);
        }
    }
}
