using Services.Interfaces;
using Models;
using Repositories;

namespace Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Room> GetRoom(string roomNumber)
        {
            return await _roomRepository.GetRoom(roomNumber);
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            return await _roomRepository.GetRooms();
        }

        public async Task<Room> CreateRoom(Room newRoom)
        {
            return await _roomRepository.CreateRoom(newRoom);
        }

        public async Task<bool> DeleteRoom(string roomNumber)
        {
            return await _roomRepository.DeleteRoom(roomNumber);
        }
    }
}
