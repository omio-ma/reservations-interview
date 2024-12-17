using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Errors;

namespace Controllers
{
    [Tags("Rooms"), Route("room")]
    public class RoomController : Controller
    {
        private IRoomService _service { get; set; }

        public RoomController(IRoomService roomRepository)
        {
            _service = roomRepository;
        }

        [HttpGet, Produces("application/json"), Route("")]
        public async Task<ActionResult<Room>> GetRooms()
        {
            var rooms = await _service.GetRooms();

            if (rooms == null)
            {
                return Json(Enumerable.Empty<Room>());
            }

            return Json(rooms);
        }

        [HttpGet, Produces("application/json"), Route("{roomNumber}")]
        public async Task<ActionResult<Room>> GetRoom(string roomNumber)
        {
            if (roomNumber.Length != 3)
            {
                return BadRequest("Invalid room ID - format is ###, ex 001 / 002 / 101");
            }

            try
            {
                var room = await _service.GetRoom(roomNumber);

                return Json(room);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost, Produces("application/json"), Route("")]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] Room newRoom)
        {
            var createdRoom = await _service.CreateRoom(newRoom);

            if (createdRoom == null)
            {
                return NotFound();
            }

            return Json(createdRoom);
        }

        [HttpDelete, Produces("application/json"), Route("{roomNumber}")]
        public async Task<IActionResult> DeleteRoom(string roomNumber)
        {
            if (roomNumber.Length != 3)
            {
                return BadRequest("Invalid room ID - format is ###, ex 001 / 002 / 101");
            }

            var deleted = await _service.DeleteRoom(roomNumber);

            return deleted ? NoContent() : NotFound();
        }
    }
}
