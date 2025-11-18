using server.DTO;
using server.IRepositories;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.models;


namespace server.Repository;

public class UserRepository:IUserRepository
{
    private ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<UserDto> RegisterUser(RegisterDTO data)
    {
        if (data.Name == null && data.Password == null)
        {
            return null;
        }

        User user = new User
        {
            Name = data.Name,
            Email = data.Email,
            UserName = data.Email, 
            Reservations = new List<Reservation>(),
            
        };
        var result = await _userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded)
        {
            return null;
        }

        //await _userManager.AddToRoleAsync(user, Roles.User);
        
        

        UserDto userDto = new UserDto
        {
            Name = user.Name,
            Email = user.Email,
            
        };


        return userDto;



    }

    
}