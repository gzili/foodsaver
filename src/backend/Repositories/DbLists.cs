using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public class DbLists
    {
        public List<User> Users { get; set; }
        public List<Food> Foods { get; set; }
        public List<Offer> Offers { get; set; }
    }
}