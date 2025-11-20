using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace server.models;

public class User:IdentityUser
{
    public string Name { get; set; }
    [JsonIgnore]
    public Token RefreshToken { get; set; } = null;
    
    public List<Reservation> Reservations { get; set; } = new();
    public string Role = Roles.User;
}