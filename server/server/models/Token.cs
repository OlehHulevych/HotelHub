using System.Text.Json.Serialization;

namespace server.models;

public class Token
{
    public int Id { get; set; }
    public string UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public string TokenString { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime ExpiresAt { get; set; }
    
}