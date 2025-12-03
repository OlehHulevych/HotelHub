using server.DTO;
using server.models;
using server.Tools;

namespace server.IRepositories;

public interface IRoomRepository
{
    
    public Task<ResultDTO> createRoom(RoomDTO data);
    public Task<ResultDTO> getRoom(int id);
    public Task<ResultDTO> updateRoom(UpdateRoomDTO data, int id);
    public Task<ResultDTO> deleteRoom(int id);
    public Task<PaginatedItemsDTO<Room>> getALlRooms(PaginationDTO pagination);
    
}