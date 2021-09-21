using System;
namespace backend.Models
{
    public class Offer
    {
        public Offer(int id, int foodId, int giverId, int quantity, DateTime expirationDate, DateTime creationDate)
        {
            Id = id;
            FoodId = foodId;
            GiverId = giverId;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            CreationDate = creationDate;
        }

        public int Id {  get; set; }
        public int FoodId {  get; set; }
        public int GiverId {  get; set; }
        public int Quantity {  get; set; }
        public DateTime ExpirationDate {  get; set; }
        public DateTime CreationDate {  get; set; }

        public override string ToString()
        {
            return Id + ", " + FoodId + ", " + GiverId + ", " + Quantity + ", " + ExpirationDate + ", " + CreationDate;
        }

        public override bool Equals(object obj)
        {
            return Id == (obj as Offer).Id;
        }
    }
}