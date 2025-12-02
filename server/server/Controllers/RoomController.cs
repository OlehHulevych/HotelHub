using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTO;
using server.Repository;

namespace server.Controllers;
[Authorize(Roles = "ADMIN")]
[Route("api/room")]
[ApiController]
public class RoomController:ControllerBase
{
    private RoomRepository _roomRepository;

    public RoomController(RoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    [HttpPost]
    public async Task<IActionResult> createPost([FromForm] RoomDTO data)
    {
        if (data == null)
        {
            return BadRequest(new { message = "There is no data" });
        }

        var response = await _roomRepository.createRoom(data);
        if (!response.result)
        {
            return BadRequest(new { message = response.Message });
        }

        return Ok(response);
    }
}