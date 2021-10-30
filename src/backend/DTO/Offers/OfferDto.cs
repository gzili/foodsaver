using System;
using backend.Models;

namespace backend.DTO.Offers
{
    public class OfferDto
    {
        public int Id { get; set; }
        public Food Food { get; set; }
        public GiverDto Giver { get; set; }
        public double Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Description { get; set; }
    }
}