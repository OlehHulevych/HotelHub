using server.DTO;
using server.IRepositories;
using BCrypt.Net;
using server.models;


namespace server.Repository;

public class UserRepository:IUserRepository
{
    public Task<UserDto> RegisterUser(RegisterDTO data)
    {
        if (data.Name == null && data.Password == null)
        {
            return null;
        }

        var passwordHashed = BCrypt.Net.BCrypt.HashPassword(data.Password);
        User user = new User
        {
            Name = data.Name,
            Password = passwordHashed,
            Email = data.Email,
            Reservations = new List<Reservation>(),
            Role = data.Role != null? data.Role: Roles.User,
            
        };

        UserDto userDto = new UserDto
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };

        return Task.FromResult(userDto);



    }

    
}