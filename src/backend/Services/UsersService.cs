using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.DTO.Address;
using backend.DTO.User;
using backend.Exceptions;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace backend.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _db;

        private IQueryable<User> Users => _db.Users
            .Include(u => u.Address);

        public UsersService(AppDbContext db)
        {
            _db = db;
        }

        public void Create(User user)
        {
            user.Password = BC.HashPassword(user.Password);
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User FindById(int id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email == email);
        }

        public User IsValidLogin(string email, string password)
        {
            var user = GetByEmail(email);
            return user != null && BC.Verify(password, user.Password) ? user : null;
        }

        public PlaceDto GetById(int id)
        {
            var placeDto = Users.Where(u => u.Id == id).Select(u => new PlaceDto
            {
                ActiveOffersCount = u.Offers.Count(o => o.ExpiresAt > DateTime.UtcNow),
                Address = new AddressDto
                {
                    City = u.Address.City,
                    Street = u.Address.Street
                },
                AvatarPath = u.AvatarPath,
                CompletedReservationsCount = u.Offers.Sum(
                    o => o.Reservations.Count(r => r.CompletedAt != null)),
                Id = u.Id,
                Name = u.Username,
                Type = u.UserType
            }).FirstOrDefault();

            if (placeDto == null)
            {
                throw new EntityNotFoundException("user", id);
            }

            return placeDto;
        }
    }
}