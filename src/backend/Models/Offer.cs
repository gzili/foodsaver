using System;
namespace backend.Models
{
    public class Offer
    {
        public Offer(int id, Food foodPtr, User giver, int quantity, DateTime expirationDate, DateTime creationDate)
        {
            Id = id;
            Food = foodPtr;
            Giver = giver;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            CreationDate = creationDate;
        }

        public int Id {  get; set; }
        public Food Food {  get; set; }
        public User Giver {  get; set; }
        public int Quantity {  get; set; }
        public DateTime ExpirationDate {  get; set; }
        public DateTime CreationDate {  get; set; }

        public override string ToString()
        {
            return Id + ", " + Food + ", " + Giver + ", " + Quantity + ", " + ExpirationDate + ", " + CreationDate;
        }

        public override bool Equals(object obj)
        {
            return Id == (obj as Offer).Id;
        }
    }
}