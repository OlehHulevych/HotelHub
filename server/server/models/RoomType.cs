namespace server.models;

public class RoomType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Room> RoomList { get; set; } = new List<Room>();
    public int Quantity { get; set; } 
}