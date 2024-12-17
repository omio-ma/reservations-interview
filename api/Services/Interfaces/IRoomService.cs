using Models;

namespace Services.Interfaces
{
    public interface IRoomService
    {
        Task<Room> GetRoom(string roomNumber);
        Task<IEnumerable<Room>> GetRooms();
        Task<Room> CreateRoom(Room newRoom);
        Task<bool> DeleteRoom(string roomNumber);
    }
}
