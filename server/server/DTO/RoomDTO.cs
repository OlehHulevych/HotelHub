namespace server.DTO;

public class RoomDTO
{
    public string Name { get; set; }
    public string RoomType { get; set; }
    public int pricePerNight { get; set; }
    public string Description { get; set; }
    public List<IFormFile> Photos { get; set; }
}