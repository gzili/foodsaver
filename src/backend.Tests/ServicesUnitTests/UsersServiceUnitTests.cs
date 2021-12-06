using System.Collections.Generic;
using backend.Data;
using backend.Models;
using backend.Services;
using backend.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using BcryptNet = BCrypt.Net.BCrypt;

namespace backend.Tests.ServicesUnitTests
{
    public class UsersServiceUnitTests
    {
        [Fact]
        public void Create_AddsAndReturnsNewUserWithHashedPassword()
        {
            const string password = "testPassword";
            var user = new User { Password = password };

            var mockSet = new Mock<DbSet<User>>();

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var service = new UsersService(mockContext.Object);
            service.Create(user);
            
            Assert.True(BcryptNet.Verify(password, user.Password));
            mockSet.Verify(s => s.Add(It.IsAny<User>()), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        
        [Fact]
        public void FindById_ReturnsUser()
        {
            var user1 = new User { Id = 1 };
            var user2 = new User { Id = 2 };
            var user3 = new User { Id = 3 };
            var data = new List<User> { user1, user2, user3 };
            
            var mockSet = MockDbSet<User>.Create(data);
            
            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet);

            var service = new UsersService(mockContext.Object);
            var returnValue = service.FindById(2);
            
            Assert.Same(user2, returnValue);
        }
        
        [Fact]
        public void FindById_ReturnsNull()
        {
            var user1 = new User { Id = 1 };
            var user2 = new User { Id = 2 };
            var user3 = new User { Id = 3 };
            var data = new List<User> { user1, user2, user3 };
            
            var mockSet = MockDbSet<User>.Create(data);
            
            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet);

            var service = new UsersService(mockContext.Object);
            var returnValue = service.FindById(4);
            
            Assert.Null(returnValue);
        }
    }
}