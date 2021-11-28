using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("offer")]
    public class Offer
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        
        public virtual User Giver { get; set; }
        public virtual Food Food { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}