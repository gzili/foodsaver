using System;
using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public class FoodRepository
    {
        private static List<Food> Foods { get; set; } = new();

        public static Food Get()
        {
            return new Food(new Random().Next(), "bandele", "labai skani", "pathpath", "vnt");
        }

        public static void Save(Food food)
        {
            Foods.Add(food);
        }

        public static List<Food> GetAll()
        {
            return Foods;
        }
    }
}