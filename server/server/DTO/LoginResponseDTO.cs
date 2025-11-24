using server.models;

namespace server.DTO;

public class LoginResponseDTO
{
    public User FoundUser { get; set; }
    public Token Token { get; set; }
    public string Error { get; set; }
}