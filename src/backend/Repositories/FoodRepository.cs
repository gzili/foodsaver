using System;
using System.Collections.Generic;
using backend.Models;
using backend.Services;

namespace backend.Repositories
{
    public class FoodRepository : IRepository<Food>
    {
        private readonly AppDbContext _appDbContext;

        public FoodRepository()
        {
            _appDbContext = AppDbContext.GetObject();
        }

        public void Save(Food food)
        {
            _appDbContext.Foods.Add(food);
        }

        public List<Food> GetAll()
        {
            return _appDbContext.Foods;
        }

        public Food GetById(int id)
        {
            return _appDbContext.Foods.Find(x => x.Id == id);
        }
    }
}