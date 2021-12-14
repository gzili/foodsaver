using System;
using System.Linq;
using backend.Data;
using backend.Exceptions;
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

        [Fact]
        public void FindById_ThrowsWhenNotFound()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var service = new ReservationsService(context);
                
                Assert.Throws<EntityNotFoundException>(() => service.FindById(100));
            }
        }

        [Fact]
        public void Create_PerformsReservationSteps()
        {
            const decimal reservationQuantity = 1;
            decimal expectedAvailableQuantity;
            int reservationId;

            using (var context = new AppDbContext(ContextOptions))
            {
                var offer = context.Offers.First(o => o.Id == 1);
                expectedAvailableQuantity = offer.AvailableQuantity - reservationQuantity;
                
                var reservation = new Reservation
                {
                    Quantity = 1,
                    Offer = offer,
                    User = context.Users.First(u => u.Id == 1)
                };

                var service = new ReservationsService(context);
                
                service.Create(reservation);

                reservationId = reservation.Id;
            }

            using (var context = new AppDbContext(ContextOptions))
            {
                var offer = context.Offers.First(o => o.Id == 1);
                
                Assert.Equal(expectedAvailableQuantity, offer.AvailableQuantity);

                var reservation = context.Reservations.First(r => r.Id == reservationId);
                
                Assert.NotEqual(0, reservation.Pin);
            }
        }

        [Fact]
        public void Create_ThrowsExceptionWhenQuantityTooLarge()
        {
            var reservation = new Reservation
            {
                Quantity = 2,
                Offer = new Offer
                {
                    AvailableQuantity = 1
                }
            };

            var service = new ReservationsService(null);

            Assert.Throws<QuantityTooLargeException>(() => service.Create(reservation));
        }

        [Fact]
        public void Create_HandlesConcurrencyExceptionAndResetsValues()
        {
            const decimal reservationQuantity = 1;
            const decimal adjustedAvailableQuantity = 4;
            const decimal expectedAvailableQuantity = adjustedAvailableQuantity - reservationQuantity;
            int reservationId;

            using (var context = new AppDbContext(ContextOptions))
            {
                var offer = context.Offers.First(o => o.Id == 1);

                var reservation = new Reservation
                {
                    Offer = offer,
                    User = context.Users.First(u => u.Id == 1),
                    Quantity = reservationQuantity
                };

                using (var innerContext = new AppDbContext(ContextOptions))
                {
                    var innerOffer = innerContext.Offers.First(o => o.Id == 1);
                    innerOffer.AvailableQuantity = 4;
                    innerContext.SaveChanges();
                }

                var service = new ReservationsService(context);
                
                service.Create(reservation);

                reservationId = reservation.Id;
            }

            using (var context = new AppDbContext(ContextOptions))
            {
                var offer = context.Offers.First(o => o.Id == 1);
                
                Assert.Equal(expectedAvailableQuantity, offer.AvailableQuantity);
                
                var reservation = context.Reservations.FirstOrDefault(r => r.Id == reservationId);
                
                Assert.NotNull(reservation);
            }
        }

        [Fact]
        public void Complete_SetsCompletionDate()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var reservation = context.Reservations.First();
                Assert.Null(reservation.CompletedAt); // Assert correct test data

                var service = new ReservationsService(context);
                
                service.Complete(reservation);
            }

            using (var context = new AppDbContext(ContextOptions))
            {
                var reservation = context.Reservations.First();
                
                Assert.NotNull(reservation.CompletedAt);
            }
        }
    }
}