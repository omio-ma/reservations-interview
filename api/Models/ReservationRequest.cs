using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ReservationRequest
    {
        [Required(ErrorMessage = "Room number is required.")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Guest email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string GuestEmail { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime End { get; set; }
    }
}
