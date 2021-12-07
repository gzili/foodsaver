﻿using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() {} // For testing only
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Offer)
                .WithMany(o => o.Reservations)
                .IsRequired();

            modelBuilder.Entity<Reservation>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("now() AT TIME ZONE 'utc'");
        }

        public virtual DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}