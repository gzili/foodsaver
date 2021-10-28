using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Offer : EntityModel, IComparable<Offer>
    {
        
        public Food Food { get; set; }
        [Column("giver")]
        public User Giver { get; set; }  // = user.id = 1
        [Column("receiver")]
        public User Receiver { get; set; } //= user.id = 2
        public double Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }

        public int CompareTo(Offer other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var quantityComparison = Quantity.CompareTo(other.Quantity);
            if (quantityComparison != 0) return quantityComparison;
            var expirationDateComparison = ExpirationDate.CompareTo(other.ExpirationDate);
            if (expirationDateComparison != 0) return expirationDateComparison;
            var creationDateComparison = CreationDate.CompareTo(other.CreationDate);
            if (creationDateComparison != 0) return creationDateComparison;
            return string.Compare(Description, other.Description, StringComparison.Ordinal);
        }
    }
}