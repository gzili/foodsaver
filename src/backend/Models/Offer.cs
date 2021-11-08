using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("offer")]
    public class Offer : IComparable<Offer>
    {
        
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        
        public virtual User Giver { get; set; }
        public virtual Food Food { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

        public int CompareTo(Offer other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            
            var quantityComparison = Quantity.CompareTo(other.Quantity);
            if (quantityComparison != 0) return quantityComparison;
            
            var expirationDateComparison = ExpiresAt.CompareTo(other.ExpiresAt);
            if (expirationDateComparison != 0) return expirationDateComparison;
            
            var creationDateComparison = CreatedAt.CompareTo(other.CreatedAt);
            if (creationDateComparison != 0) return creationDateComparison;
            
            return string.Compare(Description, other.Description, StringComparison.Ordinal);
        }
    }
}