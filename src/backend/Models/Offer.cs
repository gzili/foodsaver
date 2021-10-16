using System;

namespace backend.Models
{
    public class Offer : EntityModel
    {
        public Offer(int Id) : base(Id)
        {
        }
        
        public Food Food { get; set; }
        public User Giver { get; set; }
        public double Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
    }
}