using System;
using System.Collections.Generic;
using System.Linq;


namespace backend.DTO
{
    public class PaginatedList<T>
    {
        public List<T> List { get; private set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }

        public PaginatedList(List<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            var count = items.Count;
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);

            List = new List<T>(items);
        }

        public bool HasPreviousPage => (PageIndex > 1);

        public bool HasNextPage => (PageIndex < TotalPages);

        public List<T> GetListItemsByPage()
        {
            return List.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

        }
    }
}