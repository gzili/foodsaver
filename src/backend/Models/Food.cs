using System.Transactions;

namespace backend.Models
{
    public class Food : ModelClass
    {
        public Food(int id, string name, string description, string imagePath, string unit) : base(id)
        {
            Name = name;
            Description = description;
            ImagePath = imagePath;
            Unit = unit;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return Id + ", " + Name + ", " + Description + ", " + ImagePath + ", " + Unit;
        }
    }
}