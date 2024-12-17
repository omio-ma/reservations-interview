using FluentValidation;
using Models;

namespace Validators
{
    public class ReservationValidator : AbstractValidator<ReservationRequest>
    {
        public const string RoomNumberRequiredErrorMessage = "Room number is required.";
        public const string RoomNumberFormatErrorMessage = "Invalid room number";
        public const string GuestEmailRequiredErrorMessage = "Guest email is required.";
        public const string InvalidEmailFormatErrorMessage = "Invalid email format.";
        public const string StartDateEndDataErrorMessage = "Start date must be before the end date.";
        public const string DurationErrorMessage = "Duration must be between 1 and 30 days.";

        public ReservationValidator()
        {
            RuleFor(r => r.RoomNumber)
                .NotEmpty().WithMessage(RoomNumberRequiredErrorMessage)
                .Matches(@"^(?![1-9]00$)[1-9][0-9]{2}$").WithMessage(RoomNumberFormatErrorMessage);

            RuleFor(r => r.GuestEmail)
                .NotEmpty().WithMessage(GuestEmailRequiredErrorMessage)
                .EmailAddress().WithMessage(InvalidEmailFormatErrorMessage);

            RuleFor(r => r.Start)
                .LessThan(r => r.End).WithMessage(StartDateEndDataErrorMessage)
                .Must((r, start) => (r.End - start).TotalDays is >= 1 and <= 30)
                .WithMessage(DurationErrorMessage);
        }
    }
}
