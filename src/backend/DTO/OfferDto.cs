using System;
using backend.Models;

namespace backend.DTO
{
    public class OfferDto
    {
        public Food Food {  get; set; }
        public GiverDto Giver {  get; set; }
        public int Quantity {  get; set; }
        public DateTime ExpirationDate {  get; set; }
        public DateTime CreationDate {  get; set; }
        public string Description { get; set; }
    }
}