using Controllers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Errors;
using Moq;
using Services.Interfaces;

namespace api.unit.tests.Controllers
{
    public class ReservationControllerTests
    {
        private readonly Mock<IReservationService> _mockService;
        private readonly Mock<IValidator<ReservationRequest>> _mockValidator;
        private readonly ReservationController _controller;

        public ReservationControllerTests()
        {
            _mockService = new Mock<IReservationService>();
            _mockValidator = new Mock<IValidator<ReservationRequest>>();

            _controller = new ReservationController(
                _mockService.Object,
                _mockValidator.Object
            );
        }

        [Fact]
        public async Task CreateReservation_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var validRequest = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            var createdReservation = new Reservation
            {
                Id = Guid.NewGuid(),
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = validRequest.Start,
                End = validRequest.End,
                CheckedIn = false,
                CheckedOut = false
            };

            _mockValidator.Setup(v => v.ValidateAsync(validRequest, default))
                          .ReturnsAsync(new ValidationResult());

            _mockService.Setup(s => s.CreateReservation(validRequest))
                        .ReturnsAsync(createdReservation);

            // Act
            var result = await _controller.CreateReservation(validRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedReservation = Assert.IsType<Reservation>(createdResult.Value);

            returnedReservation.RoomNumber.Should().Be("101");
            returnedReservation.GuestEmail.Should().Be("test@example.com");
            createdResult.ActionName.Should().Be(nameof(_controller.GetReservation));
        }

        [Fact]
        public async Task CreateReservation_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new ReservationRequest
            {
                RoomNumber = "invalid-room",
                GuestEmail = "invalid-email",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(-1)
            };

            var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Error1", "Invalid room number."),
            new ValidationFailure("Error2", "Invalid email format."),
        };

            _mockValidator.Setup(v => v.ValidateAsync(invalidRequest, default))
                          .ReturnsAsync(new ValidationResult(validationErrors));

            // Act
            var result = await _controller.CreateReservation(invalidRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var errors = Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);


            errors.Should().NotBeNull();
            errors.Should().HaveCount(validationErrors.Count());
            errors.Should().ContainSingle(e => e.PropertyName == "Error1");
            errors.Should().ContainSingle(e => e.PropertyName == "Error2");
        }

        [Fact]
        public async Task CreateReservation_InvalidRoom_ThrowsInvalidRoomNumber_ReturnsBadRequest()
        {
            // Arrange
            var validRequest = new ReservationRequest
            {
                RoomNumber = "999",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockValidator.Setup(v => v.ValidateAsync(validRequest, default))
                          .ReturnsAsync(new ValidationResult());

            _mockService.Setup(s => s.CreateReservation(validRequest))
                        .ThrowsAsync(new InvalidRoomNumber(validRequest.RoomNumber));

            // Act
            var result = await _controller.CreateReservation(validRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            badRequestResult.Value.Should().Be($"The value ${validRequest.RoomNumber} is not a valid");
        }

        [Fact]
        public async Task CreateReservation_ServiceFails_ReturnsInternalServerError()
        {
            // Arrange
            var validRequest = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockValidator.Setup(v => v.ValidateAsync(validRequest, default))
                          .ReturnsAsync(new ValidationResult());

            _mockService.Setup(s => s.CreateReservation(validRequest))
                        .ThrowsAsync(new Exception("Unknown error"));

            // Act
            var result = await _controller.CreateReservation(validRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
            var serverErrorResult = Assert.IsType<ObjectResult>(actionResult.Result);
            serverErrorResult.StatusCode.Should().Be(500);
        }
    }
}
