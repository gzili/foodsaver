using System;

namespace backend.Models
{
    public class Offer : EntityModel, IComparable<Offer>
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