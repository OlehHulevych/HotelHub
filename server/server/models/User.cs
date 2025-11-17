using Microsoft.AspNetCore.Identity;

namespace server.models;

public class User:IdentityUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    

    public Token RefreshToken { get; set; } = null;
    
    public List<Reservation> Reservations { get; set; } = new();
    public string Role = Roles.User;
}