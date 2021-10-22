using System.Collections.Generic;
using backend.DTO.Offers;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class FoodService : IService<Food>
    {
        private readonly FoodRepository _foodRepository;

        public FoodService()
        {
            _foodRepository = new FoodRepository();
        }

        public Food GetById(int id)
        {
            return _foodRepository.GetById(id);
        }

        public List<Food> GetAll()
        {
            return _foodRepository.GetAll();
        }

        public void Save(Food food)
        {
            _foodRepository.Save(food);
        }

        public Food GetFromDto(OfferCreateDto offerCreateDto) => new(GetAll().Count + 1)
        {
            Name = offerCreateDto.FoodName,
            ImagePath = offerCreateDto.FoodImagePath,
            Unit = offerCreateDto.FoodUnit
        };
    }
}