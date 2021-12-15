using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Services;
using AutoMapper;
using backend.Controllers;
using Moq;
using backend.DTO;
using backend.DTO.Offer;
using backend.DTO.Reservation;
using backend.Exceptions;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace backend.Tests.ControllersUnitTests
{
    public class OffersControllerUnitTests
    {
        private readonly MockRepository _mockRepository;
        private readonly IMapper _mapper;
        private readonly User _user;
        private readonly ControllerContext _controllerContext;
        
        public OffersControllerUnitTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            
            _mapper =
                new MapperConfiguration(config => config.AddProfile<MappingProfile>())
                    .CreateMapper();

            _user = new User();

            _controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Items =
                    {
                        ["user"] = _user
                    }
                }
            };
        }
        
        [Fact]
        public void FindById_ReturnsASingleOffer()
        {
            var offer = new Offer();

            var mockOffersService =  _mockRepository.Create<IOffersService>();
            mockOffersService.Setup(s => s.FindById(1))
                .Returns(offer);

            var controller = new OffersController(mockOffersService.Object, _mapper, null, null, null);

            var result = controller.FindById(1);

            Assert.IsType<OfferDto>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequestResultOnInvalidImageFile()
        {
            var mConfiguration =  _mockRepository.Create<IConfiguration>();
            mConfiguration
                .Setup(c => c["UploadedFilesPath"])
                .Returns("testDirPath");
            var mFileService =  _mockRepository.Create<IFileService>();
            mFileService
                .Setup(fs => fs.UploadFormFileAsync(It.IsAny<IFormFile>(), "testDirPath"))
                .ReturnsAsync((string) null)
                .Verifiable();
            var createOfferDto = new CreateOfferDto();

            var controller = new OffersController(null, null, mConfiguration.Object, mFileService.Object, null);
            var result = await controller.Create(createOfferDto);

            var actionResult = Assert.IsType<ActionResult<OfferDto>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            mFileService.Verify();
        }

        [Fact]
        public async Task Create_CreatesAndReturnsAnOffer()
        {
            var createOfferDto = new CreateOfferDto
            {
                FoodPhoto = null,
                Quantity = 1
            };
            Offer argumentOffer = null;
            var mOffersService =  _mockRepository.Create<IOffersService>();
            mOffersService
                .Setup(os => os.Create(It.IsAny<Offer>()))
                .Callback<Offer>(o => argumentOffer = o)
                .Verifiable();
            var mConfiguration =  _mockRepository.Create<IConfiguration>();
            mConfiguration
                .Setup(c => c["UploadedFilesPath"])
                .Returns((string) null);
            const string returnedPath = "testFilePath";
            var mFileService =  _mockRepository.Create<IFileService>();
            mFileService
                .Setup(fs => fs.UploadFormFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync(returnedPath)
                .Verifiable();

            var controller = new OffersController(
                mOffersService.Object, _mapper, mConfiguration.Object, mFileService.Object, null)
            {
                ControllerContext = _controllerContext
            };
            
            var result = await controller.Create(createOfferDto);
            
            var actionResult = Assert.IsType<ActionResult<OfferDto>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.IsType<OfferDto>(createdAtActionResult.Value);
            Assert.Equal(createOfferDto.Quantity, argumentOffer.AvailableQuantity);
            Assert.Equal(returnedPath, argumentOffer.Food.ImagePath);
            Assert.Same(_user, argumentOffer.Giver);
            mFileService.Verify();
            mOffersService.Verify();
        }

        [Fact]
        public void Delete_CallsServiceMethod()
        {
            var offer = new Offer();
            
            var mockOffersService =  _mockRepository.Create<IOffersService>();
            mockOffersService
                .Setup(s => s.FindById(1))
                .Returns(offer);
            mockOffersService
                .Setup(os => os.Delete(offer))
                .Verifiable();

            var controller = new OffersController(mockOffersService.Object, null, null, null, null);
            controller.Delete(1);
            
            mockOffersService.Verify();
        }

        [Fact]
        public void CreateReservation_ReturnsConflictResultWhenUserAndGiverIsTheSame()
        {
            var offer = new Offer { Giver = _user };
            var mOffersService =  _mockRepository.Create<IOffersService>();
            mOffersService
                .Setup(os => os.FindById(1))
                .Returns(offer)
                .Verifiable();

            var controller = new OffersController(
                mOffersService.Object, null, null, null, null)
            {
                ControllerContext = _controllerContext
            };
            var result = controller.CreateReservation(1, null);

            var actionResult = Assert.IsType<ActionResult<ReservationDto>>(result);
            Assert.IsType<ConflictObjectResult>(actionResult.Result);
            mOffersService.Verify();
        }

        [Fact]
        public void CreateReservation_ReturnsConflictWhenAlreadyReserved()
        {
            var offer = new Offer
            {
                Reservations = new List<Reservation>
                {
                    new Reservation { User = _user }
                }
            };
            var mOffersService = _mockRepository.Create<IOffersService>();
            mOffersService
                .Setup(os => os.FindById(1))
                .Returns(offer)
                .Verifiable();

            var controller = new OffersController(
                mOffersService.Object, null, null, null, null)
            {
                ControllerContext = _controllerContext
            };
            var result = controller.CreateReservation(1, null);

            var actionResult = Assert.IsType<ActionResult<ReservationDto>>(result);
            Assert.IsType<ConflictObjectResult>(actionResult.Result);
            mOffersService.Verify();
        }

        [Fact]
        public void CreateReservation_ReturnsConflictWhenQuantityTooLargeExceptionIsThrown()
        {
            var offer = new Offer
            {
                Reservations = new List<Reservation>()
            };
            var mOffersService =  _mockRepository.Create<IOffersService>();
            mOffersService
                .Setup(os => os.FindById(1))
                .Returns(offer)
                .Verifiable();
            var mReservationsService =  _mockRepository.Create<IReservationsService>();
            mReservationsService
                .Setup(rs => rs.Create(It.IsAny<Reservation>()))
                .Throws<QuantityTooLargeException>()
                .Verifiable();

            var controller = new OffersController(
                mOffersService.Object, null, null, null, mReservationsService.Object)
            {
                ControllerContext = _controllerContext
            };
            var createReservationDto = new CreateReservationDto { Quantity = 1 };
            var result = controller.CreateReservation(1, createReservationDto);

            var actionResult = Assert.IsType<ActionResult<ReservationDto>>(result);
            Assert.IsType<ConflictObjectResult>(actionResult.Result);
            mOffersService.Verify();
            mReservationsService.Verify();
        }
        
        [Fact]
        public void CreateReservation_ReturnsValidCreatedReservation()
        {
            var offer = new Offer { Reservations = new List<Reservation>() };
            var createReservationDto = new CreateReservationDto { Quantity = 1 };
            var mOffersService =  _mockRepository.Create<IOffersService>();
            mOffersService
                .Setup(os => os.FindById(1))
                .Returns(offer)
                .Verifiable();
            Reservation argsReservation = null;
            var mReservationsService =  _mockRepository.Create<IReservationsService>();
            mReservationsService
                .Setup(rs => rs.Create(It.IsAny<Reservation>()))
                .Callback<Reservation>(r => argsReservation = r)
                .Verifiable();

            var controller = new OffersController(
                mOffersService.Object, _mapper, null, null, mReservationsService.Object)
            {
                ControllerContext = _controllerContext
            };
            var result = controller.CreateReservation(1, createReservationDto);

            var actionResult = Assert.IsType<ActionResult<ReservationDto>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.IsType<ReservationDto>(createdAtActionResult.Value);
            mOffersService.Verify();
            mReservationsService.Verify();
            Assert.Same(offer, argsReservation.Offer);
            Assert.Same(_user, argsReservation.User);
            Assert.Equal(createReservationDto.Quantity, argsReservation.Quantity);
        }

        public static IEnumerable<object[]> GetTestReservationsList()
        {
            yield return new object[] { new List<Reservation>() };
            yield return new object[] { new List<Reservation> { new() { User = new User() } } };
        }

        [Theory]
        [MemberData(nameof(GetTestReservationsList))]
        public void GetReservation_ReturnsNullWhenReservationsEmpty(List<Reservation> reservations)
        {
            var offer = new Offer { Reservations = reservations };
            var mOffersService = _mockRepository.Create<IOffersService>();
            mOffersService
                .Setup(os => os.FindById(1))
                .Returns(offer);

            var controller = new OffersController(
                mOffersService.Object, _mapper, null, null, null)
            {
                ControllerContext = _controllerContext
            };
            var result = controller.GetReservation(1);
            
            Assert.Null(result);
        }

        [Fact]
        public void GetReservation_ReturnsReservationForCurrentUser()
        {
            var offer = new Offer { Reservations = new List<Reservation> { new() { User = _user } } };
            var mOffersService = _mockRepository.Create<IOffersService>();
            mOffersService
                .Setup(os => os.FindById(1))
                .Returns(offer);

            var controller = new OffersController(
                mOffersService.Object, _mapper, null, null, null)
            {
                ControllerContext = _controllerContext
            };
            var result = controller.GetReservation(1);

            Assert.IsType<ReservationCreatorDto>(result);
        }

        [Fact]
        public void CancelReservation_ReturnsConflictWhenReservationDoesNotExist()
        {
            var offer = new Offer
            {
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        User = new User()
                    }
                }
            };
            
            var mockOffersService = new Mock<IOffersService>();
            mockOffersService.Setup(m => m.FindById(1))
                .Returns(offer);

            var controller = new OffersController(
                mockOffersService.Object, null, null, null, null)
            {
                ControllerContext = _controllerContext
            };

            var returnValue = controller.CancelReservation(1);

            Assert.IsType<ConflictObjectResult>(returnValue);
        }

        [Fact]
        public void CancelReservation_ReturnsConflictWhenReservationIsAlreadyCompleted()
        {
            var offer = new Offer
            {
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        User = _user,
                        CompletedAt = DateTime.Now
                    }
                }
            };
            
            var mockOffersService = new Mock<IOffersService>();
            mockOffersService.Setup(m => m.FindById(1))
                .Returns(offer);

            var controller = new OffersController(
                mockOffersService.Object, null, null, null, null)
            {
                ControllerContext = _controllerContext
            };

            var returnValue = controller.CancelReservation(1);

            Assert.IsType<ConflictObjectResult>(returnValue);
        }

        [Fact]
        public void CancelReservation_CallsDelete()
        {
            var reservation = new Reservation
            {
                User = _user
            };
            
            var offer = new Offer
            {
                Reservations = new List<Reservation> { reservation }
            };
            
            var mockOffersService = new Mock<IOffersService>();
            mockOffersService.Setup(m => m.FindById(1))
                .Returns(offer);

            var mockReservationsService = new Mock<IReservationsService>();

            var controller = new OffersController(
                mockOffersService.Object, null, null, null, mockReservationsService.Object)
            {
                ControllerContext = _controllerContext
            };

            var returnValue = controller.CancelReservation(1);

            Assert.IsType<OkResult>(returnValue);
            mockReservationsService.Verify(m => m.Delete(reservation), Times.Once);
        }

        [Fact]
        public void GetAllReservations_ReturnsConflictWhenNoPermission()
        {
            var offer = new Offer
            {
                Giver = new User()
            };

            var mockOffersService = new Mock<IOffersService>();
            mockOffersService
                .Setup(m => m.FindById(1))
                .Returns(offer);
            
            var controller = new OffersController(
                mockOffersService.Object, null, null, null, null)
            {
                ControllerContext = _controllerContext
            };

            var returnValue = controller.GetAllReservations(1);

            Assert.IsType<ConflictObjectResult>(returnValue.Result);
        }

        [Fact]
        public void GetAllReservations_ReturnsListOfReservationsForGiver()
        {
            var offer = new Offer
            {
                Giver = _user,
                Reservations = new List<Reservation>()
            };

            var mockOffersService = new Mock<IOffersService>();
            mockOffersService
                .Setup(m => m.FindById(1))
                .Returns(offer);
            
            var controller = new OffersController(
                mockOffersService.Object, _mapper, null, null, null)
            {
                ControllerContext = _controllerContext
            };

            var actionResult = controller.GetAllReservations(1);
            
            Assert.IsAssignableFrom<IEnumerable<ReservationDto>>(actionResult.Value);
        }
    }
}