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

    public async Task<IActionResult> getAll([FromQuery] PaginationDTO queries)
    {
        if (queries == null)
        {
            return BadRequest("There is no any queries for getting items");
        }

        var response = await _roomRepository.getALlRooms(queries);
        if (response.Items==null)
        {
            return BadRequest(new { message = "Something went wrong" });
        }

        return Ok(response);
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