using System;

namespace backend.Models
{
    public class Offer : ModelClass
    {
        public Offer(int id, Food food, User giver, int quantity, DateTime expirationDate, DateTime creationDate, string description) : base(id)
        {
            Food = food;
            Giver = giver;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            CreationDate = creationDate;
            Description = description;
        }

        public Food Food {  get; set; }
        public User Giver {  get; set; }
        public int Quantity {  get; set; }
        public DateTime ExpirationDate {  get; set; }
        public DateTime CreationDate {  get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Offer{{Id = {Id}, Giver = {Giver}, Food = {Food}, Quantity = {Quantity}, Description = {Description}, CreationDate = {CreationDate}, ExpirationDate = {ExpirationDate}";
        }
    }
}