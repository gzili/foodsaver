using System.Collections.Generic;

namespace backend.DTO.Offers
{
    public class PaginatedOfferListDto
    {
        public PaginatedOfferListDto(List<OfferDto> list, bool hasNextPage)
        {
            List = list;
            HasNextPage = hasNextPage;
        }

        public List<OfferDto> List { get; set; }
        public bool HasNextPage { get; set; }
    }
}