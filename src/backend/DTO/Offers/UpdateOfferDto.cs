using System;

namespace backend.DTO.Offers
{
    public class UpdateOfferDto
    {
        public string FoodName{ get; set; }
        public string FoodUnit{ get; set; }
        public double Quantity{ get; set; }
        public DateTime ExpiresAt{ get; set; }
        public string Description{ get; set; }
    }
}