using FluentValidation.TestHelper;
using Models;
using Validators;

namespace api.unit.tests.Validators
{
    public class ReservationValidatorTests
    {
        private readonly ReservationValidator _validator;

        public ReservationValidatorTests()
        {
            _validator = new ReservationValidator();
        }

        [Fact]
        public void Should_HaveError_When_RoomNumberIsEmpty()
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = "",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.RoomNumber)
                .WithErrorMessage(ReservationValidator.RoomNumberRequiredErrorMessage);
        }

        [Theory]
        [InlineData("000")]
        [InlineData("-101")]
        [InlineData("100")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("2020")]
        public void Should_HaveError_When_RoomNumberIsInvalid(string roomNumber)
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = roomNumber,
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.RoomNumber)
                .WithErrorMessage(ReservationValidator.RoomNumberFormatErrorMessage);
        }

        [Fact]
        public void Should_HaveError_When_GuestEmailIsEmpty()
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.GuestEmail)
                .WithErrorMessage(ReservationValidator.GuestEmailRequiredErrorMessage);
        }

        [Fact]
        public void Should_HaveError_When_GuestEmailIsInvalid()
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "invalid-email",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.GuestEmail)
                .WithErrorMessage(ReservationValidator.InvalidEmailFormatErrorMessage);
        }

        [Fact]
        public void Should_HaveError_When_StartDateIsNotBeforeEndDate()
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now.AddDays(2),
                End = DateTime.Now.AddDays(1)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Start)
                .WithErrorMessage(ReservationValidator.StartDateEndDataErrorMessage);
        }

        [Fact]
        public void Should_HaveError_When_DurationIsLessThanOneDay()
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(12)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Start)
                .WithErrorMessage(ReservationValidator.DurationErrorMessage);
        }

        [Fact]
        public void Should_HaveError_When_DurationIsGreaterThanThirtyDays()
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(31)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Start)
                .WithErrorMessage(ReservationValidator.DurationErrorMessage);
        }

        [Fact]
        public void Should_NotHaveAnyErrors_ForValidRequest()
        {
            // Arrange
            var model = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
