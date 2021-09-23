namespace backend.Models
{
    public class Food
    {
        public Food(int id, string name, string description, string image_path, string unit)
        {
            Id = id;
            Name = name;
            Description = description;
            Image_path = image_path;
            Unit = unit;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image_path { get; set; } 
        public string Unit { get; set; }

        public override string ToString()
        {
            return Id + ", " +  Name + ", " + Description + ", " + Image_path + ", " + Unit;
        }
    }
}