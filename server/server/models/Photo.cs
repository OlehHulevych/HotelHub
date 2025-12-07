namespace server.models;

public class Photo
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public string Uri { get; set; }
    public string public_id { get; set; }
}