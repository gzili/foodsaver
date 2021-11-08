using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.StateChanged += AdjustReservedAmount;
            ChangeTracker.Tracked += AdjustReservedAmount;
        }

        private void AdjustReservedAmount(object sender, EntityEntryEventArgs e)
        {
            if (e.Entry.Entity is Reservation reservation)
            {
                switch (e.Entry.State)
                {
                    case EntityState.Added:
                        reservation.Offer.ReservedQuantity += reservation.Quantity;
                        break;
                    case EntityState.Deleted:
                        reservation.Offer.ReservedQuantity -= reservation.Quantity;
                        break;
                }
            }
        }

        public DbSet<WeatherForecast> WeatherForecastSet { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Pickup> Pickups { get; set; }
    }
}