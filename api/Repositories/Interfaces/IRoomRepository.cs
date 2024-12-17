using Models;

namespace Repositories
{
    public interface IRoomRepository
    {
        Task<Room> CreateRoom(Room newRoom);
        Task<bool> DeleteRoom(string roomNumber);
        Task<Room> GetRoom(string roomNumber);
        Task<IEnumerable<Room>> GetRooms();
    }
}