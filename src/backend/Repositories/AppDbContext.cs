using System;
using System.Collections.Generic;
using System.Linq;
using backend.Models;

namespace backend.Services
{
    public class AppDbContext
    {
        private static AppDbContext _appDbContext;

        private AppDbContext()
        {
            Initialize();
        }

        public static AppDbContext GetObject()
        {
            if (_appDbContext == null)
            {
                _appDbContext = new AppDbContext();
            }

            return _appDbContext;
        }

        public List<Offer> Offers { get; set; }
        public List<User> Users { get; set; }
        public List<Food> Foods { get; set; }

        private void Initialize()
        {
            Users = new List<User> {new User(1, "edvinas@gmail.com", "edvinas", "pass", "vilnius", UserType.Individual)};
            Foods = new List<Food>
            {
                new Food(1, "bandele", "skani bandele", "path", "vnt"),
                new Food(2, "bandele", "skani bandele", "path", "kg"),
                new Food(3, "bandele", "skani bandele", "path", "litrai")
            };
            Offers = new List<Offer>
            {
                new Offer(1, GetById(Foods, 1), GetById(Users, 1), 1, DateTime.Now, DateTime.Now),
                new Offer(2, GetById(Foods, 2), GetById(Users, 1), 2, DateTime.Now, DateTime.Now),
                new Offer(3, GetById(Foods, 3), GetById(Users, 1), 3, DateTime.Now, DateTime.Now)
            };
        }

        private T GetById<T> (List<T> list, int id) where T  : ModelClass
        {
            return list.Find(x => x.Id == id);
        }
    }
}