namespace backend.Models
{
    public struct Address
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public override string ToString()
        {
            return Country + ", " + State + ", " + City + ", " + PostalCode + ", " + AddressLine1 + ", " + AddressLine2;
        }
    }
}
