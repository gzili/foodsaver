namespace backend.Models
{
    public class User : ModelClass
    {
        public User(int id, string email, string name, string password, Address address, UserType userType) : base(id)
        {
            Email = email;
            Name = name;
            Password = password;
            Address = address;
            UserType = userType;
        }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; } 
        public UserType UserType { get; set; }

        public override string ToString()
        {
            return Id + ", " + Email + ", " + Name + ", " + Password + ", " + Address + ", " + UserType;
        }
    }
}