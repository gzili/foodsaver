using System.Collections.Generic;

namespace backend.DTO
{
    public class PaginatedListDto<T>
    {
        public List<T> Data { get; set; }
        public bool HasNextPage { get; set; }
    }
}