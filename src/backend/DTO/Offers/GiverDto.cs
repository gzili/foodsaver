using backend.Models;

namespace backend.DTO.Offers
{
    public class GiverDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}