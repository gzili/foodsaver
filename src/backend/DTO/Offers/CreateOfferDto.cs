using System;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.Offers
{
    public class CreateOfferDto
    {
        public string FoodName { get; set; }
        public IFormFile FoodPhoto { get; set; }
        public string FoodUnit { get; set; }
        public double Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }
    }
}