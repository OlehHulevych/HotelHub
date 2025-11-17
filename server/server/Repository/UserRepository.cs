using server.DTO;
using server.IRepositories;
using BCrypt.Net;
using server.Data;
using server.models;


namespace server.Repository;

public class UserRepository:IUserRepository
{
    private ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<UserDto> RegisterUser(RegisterDTO data)
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

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        

        UserDto userDto = new UserDto
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };


        return userDto;



    }

    
}