namespace server.models;

public class Token
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string TokenString { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime ExpiresAt { get; set; }
    
}