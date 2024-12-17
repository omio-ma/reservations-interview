using Models;

namespace Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateReservation(Reservation newReservation);
        Task<bool> DeleteReservation(Guid reservationId);
        Task<Reservation> GetReservation(Guid reservationId);
        Task<IEnumerable<Reservation>> GetReservations();
    }
}