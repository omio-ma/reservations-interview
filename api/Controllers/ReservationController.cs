using FluentValidation;
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
        private readonly IValidator<ReservationRequest> _validator;

        public ReservationController(
            IReservationService reservationService, 
            IValidator<ReservationRequest> validator)
        {
            _service = reservationService;
            _validator = validator;
        }

        [HttpGet, Produces("application/json"), Route("")]
        public async Task<ActionResult<Reservation>> GetReservations()
        {
            var reservations = await _service.GetReservations();

            return Json(reservations);
        }

        [HttpGet, Produces("application/json"), Route("{reservationId}")]
        public async Task<ActionResult<Reservation>> GetReservation(Guid reservationId)
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
            var validationResult = await _validator.ValidateAsync(reservationRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                var createdReservation = await _service.CreateReservation(reservationRequest);
                return CreatedAtAction(nameof(GetReservation), new { reservationId = createdReservation.Id }, createdReservation);
            }
            catch (InvalidRoomNumber ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured when trying to book a reservation: {ex.Message}");
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
