using server.models;

namespace server.DTO;

public class LoginResponseDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Token Token { get; set; }
    public string Error { get; set; }
}