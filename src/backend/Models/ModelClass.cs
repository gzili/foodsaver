namespace backend.Models
{
    public abstract class ModelClass
    {
        protected ModelClass(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}