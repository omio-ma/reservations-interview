using api.Models;
using Models;

namespace api.Services
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservation(ReservationRequest request);
    }
}
