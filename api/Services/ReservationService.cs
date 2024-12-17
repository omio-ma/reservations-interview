using api.Models;
using Models;
using Models.Errors;
using Repositories;

namespace api.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly RoomRepository _roomRepository;

        public ReservationService(IReservationRepository reservationRepo, RoomRepository roomRepo)
        {
            _reservationRepository = reservationRepo;
            _roomRepository = roomRepo;
        }

        public async Task<Reservation> CreateReservation(ReservationRequest request)
        {
            var roomExists = await _roomRepository.GetRoom(request.RoomNumber);
            if (roomExists == null)
            {
                throw new InvalidRoomNumber($"Room {request.RoomNumber} does not exist.");
            }

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                RoomNumber = request.RoomNumber,
                GuestEmail = request.GuestEmail,
                Start = request.Start,
                End = request.End,
                CheckedIn = false,
                CheckedOut = false
            };

            await _reservationRepository.CreateReservation(reservation);

            return reservation;
        }
    }
}
