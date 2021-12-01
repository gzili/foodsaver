using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.DTO
{
    public class PaginatedList<T> : List<T>
    {
        private int PageIndex { get; }
        private int TotalPages { get; }

        private PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => PageIndex < TotalPages - 1;

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}