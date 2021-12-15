using System;
using System.Collections.Generic;
using System.Data.Entity;
using AutoMapper;
using backend.Controllers;
using backend.Data;
using backend.DTO;
using backend.DTO.Reservation;
using backend.Models;
using backend.Services;
using backend.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Tests.ControllersUnitTests
{
    public class ReservationsControllerUnitTests
    {
        private readonly MockRepository _mockRepository;
        private readonly IMapper _mapper;
        private readonly User _user;
        private readonly ControllerContext _controllerContext;
        private readonly Reservation _reservation;
        private readonly Mock<IReservationsService> _reservationsService;
        private readonly ReservationsController _controller;

        public ReservationsControllerUnitTests()
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
            
            _reservation = new Reservation
            {
                Id = 1,
                Offer = new Offer
                {
                    Giver = _user
                },
                Pin = 2
            };

            _reservationsService = new Mock<IReservationsService>();
            _reservationsService.Setup(s => s.FindById(1)).Returns(_reservation);
            _reservationsService.Setup(s => s.Complete(It.IsAny<Reservation>()))
                .Callback<Reservation>((r) => r.CompletedAt = DateTime.Now);

            _controller = new ReservationsController(_reservationsService.Object, _mapper)
            {
                ControllerContext = _controllerContext
            };
        }

        [Fact]
        public void Complete_CompletesAndReturnsActionResultReservationDto()
        {
            var result = _controller.Complete(1, new CompleteReservationDto {Pin = 2});

            Assert.True(result.Value.CompletedAt != null);
        }

        [Fact]
        public void Complete_WrongPinThrowsConflict()
        {
            var result = _controller.Complete(1, new CompleteReservationDto {Pin = 3}) as ActionResult<ReservationDto>;

            var actionResult = Assert.IsType<ActionResult<ReservationDto>>(result);
            Assert.IsType<ConflictObjectResult>(actionResult.Result);
        }
        
        [Fact]
        public void Complete_WrongUserThrowsConflict()
        {
            var user = new User
            {
                Id = 2
            };

            _controllerContext.HttpContext.Items.Remove("user");
            _controllerContext.HttpContext.Items.Add("user", user);
            
            var result = _controller.Complete(1, new CompleteReservationDto {Pin = 2}) as ActionResult<ReservationDto>;

            var actionResult = Assert.IsType<ActionResult<ReservationDto>>(result);
            Assert.IsType<ConflictObjectResult>(actionResult.Result);
        }
        
        [Fact]
        public void Complete_ReservationAlreadyCompletedThrowsConflict()
        {
            _reservation.CompletedAt = DateTime.Now;
            var result = _controller.Complete(1, new CompleteReservationDto {Pin = 2}) as ActionResult<ReservationDto>;

            var actionResult = Assert.IsType<ActionResult<ReservationDto>>(result);
            Assert.IsType<ConflictObjectResult>(actionResult.Result);
        }
    }
}