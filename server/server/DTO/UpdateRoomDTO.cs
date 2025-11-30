namespace server.DTO;

public class UpdateRoomDTO:RoomDTO
{
    public List<IFormFile> newPhotos { get; set; }
    public List<string> deletedPhotos { get; set; }
}