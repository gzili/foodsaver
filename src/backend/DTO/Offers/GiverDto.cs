using backend.DTO.Address;

namespace backend.DTO.Offers
{
    public class GiverDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public AddressDto Address { get; set; }
    }
}