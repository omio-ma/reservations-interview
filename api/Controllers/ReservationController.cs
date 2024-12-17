using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Errors;
using Repositories;
using Services.Interfaces;

namespace Controllers
{
    [Tags("Reservations"), Route("reservation")]
    public class ReservationController : Controller
    {
        private IReservationService _service;

        public ReservationController(IReservationRepository reservationRepository, IReservationService reservationService)
        {
            _service = reservationService;
        }

        [HttpGet, Produces("application/json"), Route("")]
        public async Task<ActionResult<Reservation>> GetReservations()
        {
            var reservations = await _service.GetReservations();

            return Json(reservations);
        }

        [HttpGet, Produces("application/json"), Route("{reservationId}")]
        public async Task<ActionResult<Reservation>> GetRepository(Guid reservationId)
        {
            try
            {
                var reservation = await _service.GetReservation(reservationId);
                return Json(reservation);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a new reservation, to generate the GUID ID on the server, send an Empty GUID (all 0s)
        /// </summary>
        /// <param name="reservationRequest"></param>
        /// <returns></returns>
        [HttpPost, Produces("application/json"), Route("")]
        public async Task<ActionResult<Reservation>> CreateReservation(
            [FromBody] ReservationRequest reservationRequest
        )
        {
            try
            {
                var createdReservation = await _service.CreateReservation(reservationRequest);
                return CreatedAtAction(nameof(CreateReservation), new { reservationId = createdReservation.Id }, createdReservation);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured when trying to book a reservation:");
                Console.WriteLine(ex.ToString());

                return BadRequest("Invalid reservation");
            }
        }

        [HttpDelete, Produces("application/json"), Route("{reservationId}")]
        public async Task<IActionResult> DeleteReservation(Guid reservationId)
        {
            var result = await _service.DeleteReservation(reservationId);

            return result ? NoContent() : NotFound();
        }
    }
}
