using backend.Models;
using backend.Repositories;
using System;
using System.Collections.Generic;

namespace backend.Services
{
    public static class OffersService
    {
        public static void Save(Offer offer)
        {
            OffersRepository.Save(offer);
        }

        public static Offer GetStandard()
        {
            return OffersRepository.GetStandard();
        }

        public static Offer GetById(int id)
        {
            return OffersRepository.GetById(id);
        }
        public static List<Offer> GetAll()
        {
            return OffersRepository.GetAll();
        }
        public static Offer GenerateRandom()
        {
            Random rng = new Random();
            return new Offer(rng.Next(1, 1000), FoodRepository.Get(), rng.Next(1, rng.Next(1, rng.Next(1, 50))), rng.Next(1, 1000), DateTime.Now, DateTime.Now);
        }
    }
}
