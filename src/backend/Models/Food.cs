namespace backend.Models
{
    public class Food : EntityModel
    {
        public Food(int id) : base(id)
        {
        }

        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Unit { get; set; }
    }
}