using Services.Interfaces;
using Models;
using Repositories;

namespace Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;

        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository;
        }

        public async Task<IEnumerable<Guest>> GetGuests()
        {
            return await _guestRepository.GetGuests();
        }
    }
}
