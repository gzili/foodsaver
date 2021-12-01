using System;
using System.IO;
using System.Linq;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext db, IConfiguration config)
        {
            if (db.Users.Any())
            {
                return;
            }

            var random = new Random();

            var path = config["UploadedFilesPath"];
            
            var users = new User[]
            {
                new()
                {
                    UserType = UserType.Individual,
                    Email = "edvinas@gmail.com",
                    Password = "$2a$12$cvSl/uRwdzPSZSHSWCfjleP6r9ShUXKuTy7eJ6IKQxSvKYYNKJsfi",
                    Username = "Edvinas",
                    Address = new Address
                    {
                        Street = "Perkūno g. 10",
                        City = "Vilniaus r."
                    }
                },
                new()
                {
                    UserType = UserType.Business,
                    Email = "andrius123@lidl.com",
                    AvatarPath = Path.Combine(path, "lidl_logo.jpg"),
                    Password = "$2a$12$vCAbO6KDewtXbU52lJAm..CoDMoTgS9b85b15Q1lk0MrTEDm3830C",
                    Username = "Lidl",
                    Address = new Address
                    {
                        Street = "Vytauto g. 111A",
                        City = "Ukmergė"
                    }
                },
                new()
                {
                    UserType = UserType.Business,
                    Email = "inga@etnodvaras.lt",
                    Password = "$2a$12$HQ2IgAaIaqvIXLX7hqP4uuzESajwdsojqiVVsRz2FSh.C22qiyQ0i",
                    Username = "Etno dvaras",
                    AvatarPath = Path.Combine(path, "etno_dvaras_logo.png"),
                    Address = new Address
                    {
                        Street = "Ukmergės g. 369",
                        City = "Vilnius"
                    }
                }
            };
            
            db.Users.AddRange(users);
            db.SaveChanges();

            var offers = new Offer[]
            {
                new()
                {
                    Quantity = 5.0m,
                    Description = "Have nowhere to put these bananas",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromHours(random.Next(1, 8))),
                    ExpiresAt = DateTime.UtcNow.AddDays(5),
                    Giver = db.Users.Find(1),
                    Address = db.Users.Include(u => u.Address).First(u => u.Id == 1).Address,
                    Food = new Food
                    {
                        Name = "Bananai",
                        Unit = "kg",
                        ImagePath = Path.Combine(path, "bananas.jpg")
                    }
                },
                new()
                {
                    Quantity = 8.0m,
                    Description = "Leftover buns from the last day",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromHours(random.Next(1, 8))),
                    ExpiresAt = DateTime.UtcNow.AddDays(2),
                    Giver = db.Users.Find(2),
                    Address = db.Users.Include(u => u.Address).First(u => u.Id == 2).Address,
                    Food = new Food
                    {
                        Name = "Marcipaninis sukutis",
                        Unit = "pcs",
                        ImagePath = Path.Combine(path, "sukutis.jpg")
                    }
                },
                new()
                {
                    Quantity = 2.0m,
                    Description = "Delicious serving made by mistake",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromHours(random.Next(1, 8))),
                    ExpiresAt = DateTime.UtcNow.AddHours(12),
                    Giver = db.Users.Find(3),
                    Address = db.Users.Include(u => u.Address).First(u => u.Id == 3).Address,
                    Food = new Food
                    {
                        Name = "Cepelinai",
                        Unit = "pcs",
                        ImagePath = Path.Combine(path, "cepelinai.jpg")
                    }
                }
            };
            
            db.Offers.AddRange(offers);
            db.SaveChanges();
        }
    }
}