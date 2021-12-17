using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;
using backend.Exceptions;
using backend.Models;
using backend.Services;
using backend.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace backend.Tests.ServicesUnitTests
{
    public class OffersServiceUnitTests
    {
        [Fact]
        public void Create_AddsAndReturnsNewOffer()
        {
            var offer = new Offer();

            var mockSet = new Mock<DbSet<Offer>>();

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet.Object);

            var service = new OffersService(mockContext.Object, null);
            service.Create(offer);

            mockSet.Verify(o => o.Add(It.IsAny<Offer>()), Times.Once);
            mockContext.Verify(o => o.SaveChanges(), Times.Once);
        }

        [Fact]
        public void FindById_ReturnsOffer()
        {
            var offer1 = new Offer { Id = 1 };
            var offer2 = new Offer { Id = 2 };

            var data = new List<Offer> { offer1, offer2 };

            var mockSet = MockDbSet<Offer>.Create(data);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet);

            var service = new OffersService(mockContext.Object, null);

            Assert.Same(offer1, service.FindById(1));
            Assert.Same(offer2, service.FindById(2));
        }

        [Fact]
        public void FindById_ThrowsEntityNotFoundException()
        {
            var offer1 = new Offer { Id = 1 };
            var offer2 = new Offer { Id = 2 };

            var data = new List<Offer> {offer1, offer2};

            var mockSet = MockDbSet<Offer>.Create(data);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet);

            var service = new OffersService(mockContext.Object, null);

            Assert.Throws<EntityNotFoundException>(() => service.FindById(7));
        }

        [Theory]
        [InlineData(true, -1, 5)]
        [InlineData(true, 0, 30)]
        [InlineData(true, 1, -1)]
        public void FindAllPaginated_ReturnsNormalizedSize(bool includeExpired, int page, int limit)
        {
            var data = Enumerable.Range(0, 30).Select(_ => new Offer()).ToList();

            var mockSet = MockDbSet<Offer>.Create(data);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet);

            var service = new OffersService(mockContext.Object, null);

            var size = service.FindAllPaginated(includeExpired, page, limit, 0).Count;

            Assert.True(size is > 0 and <= 25);
        }

        [Theory]
        [InlineData(true, 0, 6)]
        [InlineData(true, 0, 3)]
        public void FindAllPaginated_ReturnPaginatedSizedList(bool includeExpired, int page, int limit)
        {
            var data = Enumerable.Range(0, 6).Select(_ => new Offer()).ToList();

            var mockSet = MockDbSet<Offer>.Create(data);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet);

            var service = new OffersService(mockContext.Object, null);

            var size = service.FindAllPaginated(includeExpired, page, limit, 0).Count;

            Assert.True(size == limit);
        }

        [Fact]
        public void FindAllPaginated_ReturnPaginatedIndexedList()
        {
            var data = Enumerable.Range(0, 6).Select(_ => new Offer()).ToList();

            var mockSet = MockDbSet<Offer>.Create(data);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet);

            var service = new OffersService(mockContext.Object, null);

            var size = service.FindAllPaginated(true, 1, 5, 0).Count;

            Assert.True(size == 1);
        }

        [Fact]
        public void FindAllPaginated_ReturnOnlyNotExpired()
        {
            var offer1 = new Offer {Id = 1, ExpiresAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(10))};
            var offer2 = new Offer {Id = 2, ExpiresAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(10))};
            var offer3 = new Offer {Id = 3, ExpiresAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(10))};
            var offer4 = new Offer {Id = 4, ExpiresAt = DateTime.Now.AddMinutes(10)};
            var offer5 = new Offer {Id = 5, ExpiresAt = DateTime.Now.AddMinutes(10)};
            var offer6 = new Offer {Id = 6, ExpiresAt = DateTime.Now.AddMinutes(10)};

            var data = new List<Offer> {offer1, offer2, offer3, offer4, offer5, offer6};

            var mockSet = MockDbSet<Offer>.Create(data);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet);

            var service = new OffersService(mockContext.Object, null);

            var size = service.FindAllPaginated(false, 0, 5, 0).Count;

            Assert.True(size == 3);
        }

        [Fact]
        public void Delete_DeletesOffer()
        {
            var food = new Food
            {
                Id = 1,
                ImagePath = "path"
            };

            var offer = new Offer {Id = 1, Food = food};

            var data = new List<Offer> {offer};

            var mockSet = MockDbSet<Offer>.Create(data);

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Offers).Returns(mockSet);

            var mockFileService = new Mock<IFileService>();

            var service = new OffersService(mockContext.Object, mockFileService.Object);

            service.Delete(offer);

            mockContext.Verify(o => o.Offers.Remove(offer), Times.Once);
            mockContext.Verify(o => o.SaveChanges(), Times.Once);
        }
    }
}