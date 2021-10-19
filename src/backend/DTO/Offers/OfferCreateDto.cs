using System;
using backend.Models;

namespace backend.DTO.Offers
{
    public class OfferCreateDto
    {
        public string FoodName { get; set; }
        public string FoodImagePath { get; set; }
        public string FoodUnit { get; set; }
        public double Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }
    }
}