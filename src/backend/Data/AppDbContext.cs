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

        private static void AdjustReservedAmount(object sender, EntityEntryEventArgs e)
        {
            switch (e.Entry.Entity)
            {
                case Reservation reservation:
                    switch (e.Entry.State)
                    {
                        case EntityState.Added:
                            reservation.Offer.AvailableQuantity -= reservation.Quantity;
                            break;
                        case EntityState.Deleted:
                            reservation.Offer.AvailableQuantity += reservation.Quantity;
                            break;
                    }

                    break;
                case Offer offer when e.Entry.State == EntityState.Added:
                    offer.AvailableQuantity = offer.Quantity;
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Offer)
                .WithMany(o => o.Reservations)
                .IsRequired();
        }

        public DbSet<WeatherForecast> WeatherForecastSet { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Pickup> Pickups { get; set; }
    }
}