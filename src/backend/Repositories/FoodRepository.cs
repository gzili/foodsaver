using System;
using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public class FoodRepository : IRepository<Food>
    {
        private readonly AppDbContext _appDbContext;

        public FoodRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Save(Food food)
        {
            throw new NotImplementedException();
        }

        public List<Food> GetAll()
        {
            throw new NotImplementedException();
        }

        public Food GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}