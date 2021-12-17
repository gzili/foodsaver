using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace backend.Tests.Mocks
{
    public static class MockDbSet<T> where T : class
    {
        public static DbSet<T> Create(IEnumerable<T> data)
        {
            var queryableData = data.AsQueryable();
            
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>()
                .Setup(s => s.Provider)
                .Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>()
                .Setup(s => s.Expression)
                .Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>()
                .Setup(s => s.ElementType)
                .Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>()
                .Setup(s => s.GetEnumerator())
                .Returns(queryableData.GetEnumerator());

            return mockSet.Object;
        }
    }
}