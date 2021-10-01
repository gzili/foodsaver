namespace backend.Models
{
    public struct Address
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return $"Address{{StreetAddress = {StreetAddress}, City = {City}}}";
        }
    }
}
