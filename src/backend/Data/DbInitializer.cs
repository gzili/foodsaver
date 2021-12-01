using System;
using System.IO;
using System.Linq;
using backend.Models;
using Microsoft.Extensions.Configuration;
using BCryptNet = BCrypt.Net.BCrypt;

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
                    Password = BCryptNet.HashPassword("pass"),
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
                    Password = BCryptNet.HashPassword("parkside"),
                    Username = "Lidl Ukmergė",
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
                    Password = BCryptNet.HashPassword("manozepelinas"),
                    Username = "Etno dvaras BIG",
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
                    Giver = users[0],
                    Address = users[0].Address,
                    Food = new Food
                    {
                        Name = "Bananas",
                        Unit = "kg",
                        MinQuantity = 0.5m,
                        ImagePath = Path.Combine(path, "bananas.jpg")
                    }
                },
                new()
                {
                    Quantity = 8.0m,
                    Description = "Leftover buns from the last day",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromHours(random.Next(1, 8))),
                    ExpiresAt = DateTime.UtcNow.AddDays(2),
                    Giver = users[1],
                    Address = users[1].Address,
                    Food = new Food
                    {
                        Name = "Marcipaninis sukutis",
                        Unit = "pcs",
                        MinQuantity = 1,
                        ImagePath = Path.Combine(path, "sukutis.jpg")
                    }
                },
                new()
                {
                    Quantity = 2.0m,
                    Description = "Delicious serving made by mistake",
                    CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromHours(random.Next(1, 8))),
                    ExpiresAt = DateTime.UtcNow.AddHours(12),
                    Giver = users[2],
                    Address = users[2].Address,
                    Food = new Food
                    {
                        Name = "Zeppelins",
                        Unit = "pcs",
                        MinQuantity = 1,
                        ImagePath = Path.Combine(path, "cepelinai.jpg")
                    }
                }
            };
            
            db.Offers.AddRange(offers);
            db.SaveChanges();
        }
    }
}