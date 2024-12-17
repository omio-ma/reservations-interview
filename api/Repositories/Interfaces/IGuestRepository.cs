using Models;

namespace Repositories
{
    public interface IGuestRepository
    {
        Task<Guest> GetGuestByEmail(string guestEmail);
        Task<IEnumerable<Guest>> GetGuests();
        Task<Guest> CreateGuest(Guest newGuest);
        Task<bool> DeleteGuestByEmail(string guestEmail);
    }
}