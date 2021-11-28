using backend.DTO.Address;

namespace backend.DTO.Offer
{
    public class GiverDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AvatarPath { get; set; }
        public AddressDto Address { get; set; }
    }
}