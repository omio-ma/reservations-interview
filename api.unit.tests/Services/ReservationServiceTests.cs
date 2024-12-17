using FluentAssertions;
using Models;
using Models.Errors;
using Moq;
using Repositories;
using Services;
using Services.Errors;

namespace api.unit.tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _mockReservationRepo;
        private readonly Mock<IRoomRepository> _mockRoomRepo;
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _mockReservationRepo = new Mock<IReservationRepository>();
            _mockRoomRepo = new Mock<IRoomRepository>();
            _service = new ReservationService(_mockReservationRepo.Object, _mockRoomRepo.Object);
        }

        [Fact]
        public async Task CreateReservation_ValidRequest_ReturnsReservation()
        {
            // Arrange
            var request = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            var mockRoom = new Room { Number = "101", State = State.Ready };

            _mockRoomRepo.Setup(r => r.GetRoom(request.RoomNumber))
                         .ReturnsAsync(mockRoom);

            _mockReservationRepo.Setup(r => r.CreateReservation(It.IsAny<Reservation>()))
                                .ReturnsAsync(new Reservation
                                {
                                    Id = Guid.NewGuid(),
                                    RoomNumber = "101",
                                    GuestEmail = "test@example.com",
                                    Start = request.Start,
                                    End = request.End
                                });

            // Act
            var result = await _service.CreateReservation(request);

            // Assert
            result.Should().NotBeNull();
            result.RoomNumber.Should().Be("101");
            result.GuestEmail.Should().Be("test@example.com");
            result.Start.Should().Be(request.Start);
            result.End.Should().Be(request.End);
        }

        [Fact]
        public async Task CreateReservation_InvalidRoom_ThrowsInvalidRoomNumber()
        {
            // Arrange
            var request = new ReservationRequest
            {
                RoomNumber = "999",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockRoomRepo.Setup(r => r.GetRoom(request.RoomNumber))
                         .ThrowsAsync(new InvalidRoomNumber("999"));

            // Act & Assert
            var act = async () => await _service.CreateReservation(request);

            await act.Should().ThrowAsync<InvalidRoomNumber>();
        }

        [Fact]
        public async Task CreateReservation_DoubleBooking_ThrowsDoubleBookException()
        {
            // Arrange
            var request = new ReservationRequest
            {
                RoomNumber = "101",
                GuestEmail = "test@example.com",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            var mockRoom = new Room { Number = "101", State = State.Ready };

            _mockRoomRepo.Setup(r => r.GetRoom(request.RoomNumber))
                         .ReturnsAsync(mockRoom);

            _mockReservationRepo.Setup(r => r.IsRoomDoubleBooked(int.Parse(request.RoomNumber), request.Start, request.End))
                                .ReturnsAsync(true);

            // Act
            var act = async () => await _service.CreateReservation(request);

            // Assert
            await act.Should().ThrowAsync<DoubleBookException>();
        }
    }
}
