using System;
using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace backend.Tests.ServicesUnitTests
{
    public abstract class ReservationsServiceUnitTests
    {
        protected DbContextOptions<AppDbContext> ContextOptions { get; }

        protected ReservationsServiceUnitTests(DbContextOptions<AppDbContext> options)
        {
            ContextOptions = options;
            
            Seed();
        }

        private void Seed()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var address = new Address
                {
                    Id = 1,
                    City = string.Empty,
                    Street = string.Empty
                };
                
                var user = new User
                {
                    Id = 1,
                    UserType = UserType.Individual,
                    Email = string.Empty,
                    Username = string.Empty,
                    Password = string.Empty,
                    Address = address
                };

                var offer = new Offer
                {
                    Id = 1,
                    Quantity = 8,
                    AvailableQuantity = 6,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(5),
                    Giver = user,
                    Food = new Food
                    {
                        Name = string.Empty,
                        ImagePath = string.Empty,
                        Unit = string.Empty,
                        MinQuantity = 1
                    }
                };

                var reservation = new Reservation
                {
                    Id = 1,
                    CreatedAt = DateTime.UtcNow,
                    User = user,
                    Offer = offer,
                    Quantity = 2
                };
                
                context.AddRange(user, offer, reservation);
                context.SaveChanges();
            }
        }

        [Fact]
        public void FindById_ReturnsReservation()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var service = new ReservationsService(context);

                var reservation = service.FindById(1);
                
                Assert.Equal(1, reservation.Id);
            }
        }
    }
}