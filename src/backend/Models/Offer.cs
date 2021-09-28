using System;
namespace backend.Models
{
    public class Offer : ModelClass
    {
        public Offer(int id, Food food, User giver, int quantity, DateTime expirationDate, DateTime creationDate) : base(id)
        {
            Food = food;
            Giver = giver;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            CreationDate = creationDate;
        }

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