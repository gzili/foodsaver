using System;
using backend.Models;

namespace backend.DTO.Offers
{
    public class OfferCreateDto
    {
        public class FoodDtoClass
        {
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public string Unit { get; set; }
        }
        
        public FoodDtoClass FoodDto { get; set; }
        public double Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Description { get; set; }
    }
}