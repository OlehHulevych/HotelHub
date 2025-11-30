using server.DTO;

namespace server.IRepositories;

public interface IRoomRepository
{
    
    public Task<ResultDTO> createRoom(RoomDTO data);
    public Task<ResultDTO> getRoom(int id);
    public Task<ResultDTO> updateRoom(UpdateRoomDTO data, int id);
    public Task<ResultDTO> deleteRoom(int id);
    public List<Object> getALlRooms(string type, string category, string groupBy, int page);
    
}