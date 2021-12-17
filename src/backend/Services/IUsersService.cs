using System.Collections.Generic;
using backend.DTO.User;
using backend.Models;

namespace backend.Services
{
    public interface IUsersService
    {
        void Create(User user);
        User FindById(int id);
        User GetByEmail(string email);
        User IsValidLogin(string email, string password);
        PlaceDto GetById(int id);
    }
}