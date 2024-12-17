using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [Tags("Guests"), Route("guest")]
    public class GuestController : Controller
    {
        private IGuestService _service;

        public GuestController(IGuestService guestService)
        {
            _service = guestService;
        }

        [HttpGet, Produces("application/json"), Route("")]
        public async Task<ActionResult<Guest>> GetGuests()
        {
            var guests = await _service.GetGuests();

            return Json(guests);
        }
    }
}
