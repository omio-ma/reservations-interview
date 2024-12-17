using Models;

namespace Services.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetReservations();
        Task<Reservation> GetReservation(Guid reservationId);
        Task<Reservation> CreateReservation(ReservationRequest request);
        Task<bool> DeleteReservation(Guid reservationId);
    }
}
