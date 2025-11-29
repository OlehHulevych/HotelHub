using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.DTO;
using server.models;
using server.Repository;

namespace server.Controllers;
[Authorize(Roles = "ADMIN")]
[Route("api/RoomType")]
[ApiController]
public class RoomTypeController:ControllerBase
{
    public ApplicationDbContext _context;
    public RoomTypeRepository _roomTypeRepository;

    public RoomTypeController(ApplicationDbContext context, RoomTypeRepository roomTypeRepository)
    {
        _context = context;
        _roomTypeRepository = roomTypeRepository;
    }

    [HttpPost]
    public async Task<IActionResult> createRoomType([FromForm] RoomTypeDTO data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            return BadRequest(new { message = "There is no name of room type" });
        }

        var result = await _roomTypeRepository.AddRoomType(data);
        if (!result.result)
        {
            return BadRequest(new {message = "Something went wrong"});
        }

        return Ok(result);

    }
}