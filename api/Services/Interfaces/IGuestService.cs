using Models;

namespace Services.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetGuests();
    }
}
