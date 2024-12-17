namespace Services.Errors
{
    public class DoubleBookException : Exception
    {
        public DoubleBookException(int roomNumber, DateTime start, DateTime end)
          : base($"Room {roomNumber} is already booked between {start:yyyy-MM-dd} and {end:yyyy-MM-dd}.")
        {
        }
    }
}
