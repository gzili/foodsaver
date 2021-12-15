using System;
using System.Data.Common;
using backend.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace backend.Tests.ServicesUnitTests
{
    public class ReservationsServiceUnitTestsSqliteInMemory : ReservationsServiceUnitTests, IDisposable
    {
        private readonly DbConnection _connection;
        
        public ReservationsServiceUnitTestsSqliteInMemory()
            : base(
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseLazyLoadingProxies()
                    .UseSqlite(CreateInMemoryDb())
                    .Options
                )
        {
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDb()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        public void Dispose() => _connection.Dispose();
    }
}