﻿using System.Collections.Generic;
using backend.Models;

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
            _appDbContext.DbLists.Foods.Add(food);
        }

        public List<Food> GetAll()
        {
            return _appDbContext.DbLists.Foods;
        }

        public Food GetById(int id)
        {
            return _appDbContext.DbLists.Foods.Find(x => x.Id == id);
        }
    }
}