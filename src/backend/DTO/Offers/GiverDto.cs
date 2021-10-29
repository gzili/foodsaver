using backend.DTO.Address;

namespace backend.DTO.Offers
{
    public class GiverDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
    }
}