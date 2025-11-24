using server.DTO;

namespace server.IRepositories;

public interface IUserRepository
{
    Task<UserDto> RegisterUser(RegisterDTO data);
    Task<LoginResponseDTO> LoginUser(LoginDTO data);
    Task<LogoutDTO> LogoutUser(string email);
    Task<ResultDTO> ChangeUserPassword(string id, ChnagePasswordDTO model);
}