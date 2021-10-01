namespace backend.Models
{
    public class Food : ModelClass
    {
        public Food(int id, string name, string imagePath, string unit) : base(id)
        {
            Name = name;
            ImagePath = imagePath;
            Unit = unit;
        }

        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return $"Food{{Id = {Id}, Name = {Name}, Unit = {Unit}, ImagePath = {ImagePath}";
        }
    }
}