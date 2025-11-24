using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using server.Data;

using server.DTO;
using server.Repository;
using server.Tools;

namespace server.Controllers;

[Route("api/user")]
public class UserController : Controller
{
    private ApplicationDbContext _context;

    private JwtTokenService _jwtTokenService;
    private UserRepository _userRepository;

    public UserController(ApplicationDbContext context, JwtTokenService jwtTokenService, UserRepository userRepository)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _userRepository = userRepository;
    }
    // GET

    [HttpPost("register")]
    public async Task<IActionResult> register([FromForm] RegisterDTO data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("The body was wrong");
        }

        var result = await _userRepository.RegisterUser(data);
        if (result == null)
        {
            return BadRequest("Something went wrong");
        }

        return Ok(result);



    }
    [HttpPost("login")]
    public async Task<IActionResult> login([FromForm] LoginDTO data)
    {
        if (string.IsNullOrWhiteSpace(data.Email) && string.IsNullOrWhiteSpace(data.Password))
        {
            return BadRequest("Something is missing");
        }

        var result = await _userRepository.LoginUser(data);
        if (result.FoundUser == null || result.Token == null)
        {
            return BadRequest(result.Error);
        }
        Response.Cookies.Append("refreshToken",result.Token.TokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        } );
        HttpContext.Session.SetString("UserId",result.FoundUser.Id );
        
        return Ok(result);
    }
    [HttpGet("logout")]
    public async Task<IActionResult> logout()
    {
        string userId = HttpContext.Session.GetString("UserId");
        if (userId == null)
        {
            return BadRequest(new LogoutDTO
            {
                Error = "The id of user is not available"
            });
        }

        bool result = await _jwtTokenService.DestroyToken(userId);
        if (!result)
        {
            return BadRequest("Something went wrong");
        }
        HttpContext.Session.Remove("UserId");
        return Ok("The use is loged out");
    }
    
    [HttpPost("changePasword")]
    public async Task<IActionResult> ChangePassword([FromForm] ChnagePasswordDTO model)
    {
        string UserId = HttpContext.Session.GetString("UserId");
        Console.WriteLine("This is id: "+UserId);
        if (UserId == null)
        {
            return Unauthorized("The user is not authorized");
        }

        var result = await _userRepository.ChangeUserPassword(UserId, model);
        if(!result.result)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);

    }
}