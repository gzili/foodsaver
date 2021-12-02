using backend.Services;
using AutoMapper;
using backend.Controllers;
using Moq;
using backend.DTO;
using backend.DTO.Offer;
using backend.Models;
using Xunit;

namespace backend.Tests.ControllersUnitTests
{
    public class OffersControllerUnitTests
    {
        [Fact]
        public void FindById_ReturnsASingleOffer()
        {
            var offer = new Offer();
            
            var config = new MapperConfiguration(c => c.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();
            var mockOffersService = new Mock<IOffersService>(MockBehavior.Strict);
            mockOffersService.Setup(s => s.FindById(1))
                .Returns(offer);

            var controller = new OffersController(
                null, null, mapper, mockOffersService.Object, null);

            var result = controller.FindById(1);

            Assert.IsType<OfferDto>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_CallsServiceMethod()
        {
            var offer = new Offer();
            
            var mockOffersService = new Mock<IOffersService>();
            mockOffersService.Setup(s => s.FindById(1))
                .Returns(offer);

            var controller = new OffersController(
                null, null, null, mockOffersService.Object, null);

            controller.Delete(1);
            
            mockOffersService.Verify(s => s.Delete(offer), Times.Once);
        }
    }
}