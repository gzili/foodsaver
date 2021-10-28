namespace backend.Models
{
    public class Food : EntityModel
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Unit { get; set; }
    }
}