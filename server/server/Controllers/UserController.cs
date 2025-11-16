using Microsoft.AspNetCore.Mvc;
using server.Data;

using server.DTO;
using server.Tools;

namespace server.Controllers;

[Route("api/user")]
public class UserController : Controller
{
    private ApplicationDbContext _context;

    private JwtTokenService _jwtTokenService;

    public UserController(ApplicationDbContext context, JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }
    // GET

    [HttpPost("register")]
    public Task<IActionResult> register([FromBody] RegisterDTO data)
    {
        if (!ModelState.IsValid)
        {
            return Task.FromResult<IActionResult>(BadRequest("The body was wrong"));
        }

        return null;

    }
}