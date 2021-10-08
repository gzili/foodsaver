using System;
using backend.Models;

namespace backend.DTO.Offers
{
    public class OfferDto
    {
        public int Id { get; set; }
        public Food Food { get; set; }
        public GiverDto Giver { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
    }
}