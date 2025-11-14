namespace server.models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PricePerNight { get; set; }
    public string Description { get; set; }
    public List<string> Photos { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
}