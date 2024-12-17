using Models;
using Models.Errors;
using Repositories;
using Services.Errors;
using Services.Interfaces;

namespace Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;

        public ReservationService(IReservationRepository reservationRepo, IRoomRepository roomRepo)
        {
            _reservationRepository = reservationRepo;
            _roomRepository = roomRepo;
        }

        public async Task<Reservation> GetReservation(Guid reservationId)
        {
            return await _reservationRepository.GetReservation(reservationId);
        }

        public async Task<IEnumerable<Reservation>> GetReservations()
        {
            return await _reservationRepository.GetReservations();
        }

        public async Task<Reservation> CreateReservation(ReservationRequest request)
        {
            var roomExists = await _roomRepository.GetRoom(request.RoomNumber);
            if (roomExists == null)
            {
                throw new InvalidRoomNumber(request.RoomNumber);
            }
            var roomNumberFormatted = Room.ConvertRoomNumberToInt(request.RoomNumber);

            var isDoubleBooked = await _reservationRepository.IsRoomDoubleBooked(roomNumberFormatted, request.Start, request.End);
            if (isDoubleBooked)
            {
                throw new DoubleBookException(roomNumberFormatted, request.Start, request.End);
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

        public async Task<bool> DeleteReservation(Guid reservationId)
        {
            return await _reservationRepository.DeleteReservation(reservationId);
        }

    }
}
