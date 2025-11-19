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
    public async Task<IActionResult> register([FromBody] RegisterDTO data)
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
    public async Task<IActionResult> login([FromBody] LoginDTO data)
    {
        if (string.IsNullOrWhiteSpace(data.Email) && string.IsNullOrWhiteSpace(data.Password))
        {
            return BadRequest("Something is missing");
        }

        var result = await _userRepository.LoginUser(data);
        if (result.Email == null || result.Name == null || result.Token == null)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}