using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using server.DTO;
using server.Repository;

namespace server.Controllers;
[Route("api/reservation")]
public class ReservationController:ControllerBase
{
    private readonly ReservationRepository _reservationRepository;

    public ReservationController(ReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> postReservation([FromForm] ReservationDTO data)
    {
        if (data == null)
        {
            return BadRequest("There is no data for reservation");
        }

        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _reservationRepository.createReservation(data, id);
        if (!response.result)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> updateReservation([FromBody] UpdateReservationDTO data, [FromQuery] int id )
    {
        if (data == null)
        {
            return BadRequest("There is not data for editing");
        }

        var response = await _reservationRepository.editReservation(data, id);
        if (!response.result)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> deleteReservation([FromQuery] int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await _reservationRepository.deleteReservation(id, userId);
        if (!response.result)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }
    
    

}