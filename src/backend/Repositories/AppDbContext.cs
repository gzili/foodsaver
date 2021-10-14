using System;
using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
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
            return _appDbContext ??= new AppDbContext();
        }

        public List<Offer> Offers { get; set; }
        public List<User> Users { get; set; }
        public List<Food> Foods { get; set; }

        public Object this[string fieldName]
        {
            get 
            { 
                if (Equals(fieldName, "offers"))
                {
                    return Offers;
                }
                else if (Equals(fieldName, "users"))
                {
                    return Users;
                }
                else if (Equals(fieldName, "foods"))
                {
                    return Foods;
                }
                else
                {
                    return null;
                }
            }
        }

        private void Initialize()
        {
            Users = new List<User>
            {
                new User(1, "edvinas@gmail.com", "Edvinas", "$2a$12$cvSl/uRwdzPSZSHSWCfjleP6r9ShUXKuTy7eJ6IKQxSvKYYNKJsfi", new Address{StreetAddress = "Perkūno g. 10", City = "Vilniaus r."}, UserType.Individual), //pass -> pass
                new User(2, "andrius123@lidl.com", "Lidl", "$2a$12$vCAbO6KDewtXbU52lJAm..CoDMoTgS9b85b15Q1lk0MrTEDm3830C", new Address{StreetAddress = "Vytauto g. 111A", City = "Ukmergė"}, UserType.Enterprise), //pass -> parkside
                new User(3, "inga@etnodvaras.lt", "Etno dvaras", "$2a$12$HQ2IgAaIaqvIXLX7hqP4uuzESajwdsojqiVVsRz2FSh.C22qiyQ0i", new Address{StreetAddress = "Ukmergės g. 369", City = "Vilnius"}, UserType.Enterprise) //pass -> manozepelinas
            };
            Foods = new List<Food>
            {
                new Food(1, "Marcipaninis sukutis", "images/sukutis.jpg", "vnt."),
                new Food(2, "Bananai", "images/bananas.jpg", "kg"),
                new Food(3, "Cepelinai", "images/cepelinai.jpg", "vnt."),
                new Food(4, "Pizzas", "", "boxes")
            };
            Offers = new List<Offer>
            {
                new Offer(1, GetById(Foods, 1), GetById(Users, 2), 3, DateTime.Now, DateTime.Now, "Leftover buns from the last day"),
                new Offer(2, GetById(Foods, 2), GetById(Users, 1), 1, DateTime.Now, DateTime.Now, "Have nowhere to put these bananas"),
                new Offer(3, GetById(Foods, 3), GetById(Users, 3), 2, DateTime.Now, DateTime.Now, "Delicious serving made by mistake"),
                new Offer(2, GetById(Foods, 4), GetById(Users, 1), 4, DateTime.Now, DateTime.Now, "Giving away a few pizzas due to cancelled party")
            };
        }

        private T GetById<T> (List<T> list, int id) where T : EntityModel
        {
            return list.Find(x => x.Id == id);
        }
    }
}