using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using server.DTO;
using server.IRepositories;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.models;
using server.Tools;


namespace server.Repository;

public class UserRepository:IUserRepository
{
    private ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly JwtTokenService _jwtTokenService;

    public UserRepository(ApplicationDbContext context, UserManager<User> userManager, JwtTokenService jwtTokenService)
    {
        _context = context;
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
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

        await _userManager.AddToRoleAsync(user, Roles.User);

        UserDto userDto = new UserDto
        {
            Name = user.Name,
            Email = user.Email,
            
        };
        


        return userDto;



    }

    public async Task<LoginResponseDTO> LoginUser(LoginDTO data)
    {
        if (string.IsNullOrWhiteSpace(data.Email) && string.IsNullOrWhiteSpace(data.Password))
        {
            return new LoginResponseDTO
            {
                Error = "Email and password are required."
            };
           ;
        }

        var foundUser = await _userManager.FindByEmailAsync(data.Email);
        foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(foundUser))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(foundUser);
            Console.WriteLine("{0}={1}", name, value);
        }
        if (foundUser==null)
        {
            return new LoginResponseDTO
            {
                Error = "The user is not found"
            };
        }

        var roles = await _userManager.GetRolesAsync(foundUser);
        var token = await _jwtTokenService.CreateToken(foundUser);
        if (token == null)
        {
            return new LoginResponseDTO
            {
                Error = "Something went wrong during creating token"
            };
        }

        var validPassword = await _userManager.CheckPasswordAsync(foundUser, data.Password);
        if (!validPassword)
        {
            return new LoginResponseDTO
            {
                Error = "Password is invalid"
            };
        }

        return new LoginResponseDTO
        {
            Email = foundUser.Email,
            Name = foundUser.Name,
            Token = token
        };

    }

    public async Task<LogoutDTO> LogoutUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new LogoutDTO
            {
                Result = false,
                Error = "The user is not found"
            };
        }

        var token = await _context.Tokens.FirstOrDefaultAsync(item => item.UserId == user.Id);
        if (token == null)
        {
            return new LogoutDTO
            {
                Result = false,
                Error = "Token is not found"
            };
        }

        _context.Tokens.Remove(token);
        await _context.SaveChangesAsync();
        return new LogoutDTO
        {
            Result = true,
        };

    }

    
}