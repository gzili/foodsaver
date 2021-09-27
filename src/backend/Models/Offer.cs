using System;
namespace backend.Models
{
    public class Offer
    {
        public Offer(int id, Food foodPtr, int giverId, int quantity, DateTime expirationDate, DateTime creationDate)
        {
            Id = id;
            FoodPtr = foodPtr;
            GiverId = giverId;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            CreationDate = creationDate;
        }

        public int Id {  get; set; }
        public Food FoodPtr {  get; set; }
        public int GiverId {  get; set; }
        public int Quantity {  get; set; }
        public DateTime ExpirationDate {  get; set; }
        public DateTime CreationDate {  get; set; }

        public override string ToString()
        {
            return Id + ", " + FoodPtr + ", " + GiverId + ", " + Quantity + ", " + ExpirationDate + ", " + CreationDate;
        }

        public override bool Equals(object obj)
        {
            return Id == (obj as Offer).Id;
        }
    }
}