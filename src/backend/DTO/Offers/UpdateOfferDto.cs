using System;

namespace backend.DTO.Offers
{
    public class UpdateOfferDto
    {
        public double Quantity{ get; set; }
        public DateTime ExpiresAt{ get; set; }
        public string Description{ get; set; }
    }
}