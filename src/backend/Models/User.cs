namespace backend.Models
{
    public class User : EntityModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; }
        public UserType UserType { get; set; }
    }
}