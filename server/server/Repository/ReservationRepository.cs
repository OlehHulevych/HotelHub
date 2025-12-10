using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTO;
using server.IRepositories;

namespace server.Repository;

public class ReservationRepository:IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ResultDTO> createReservation(ReservationDTO data)
    {
        var userCheckOut = data.CheckOut;
        var userCheckIn = data.CheckIn;
        var userDiff = userCheckOut.Subtract(userCheckIn);
        var userRoom = await _context.Rooms.Include(r=>r.Reservations).FirstOrDefaultAsync(r=>r.Id == data.RoomId);
        if (userRoom == null)
        {
            return new ResultDTO
            {
                result = false,
                Message = "The room is not found"
            };
        }

        var RoomReservation = userRoom.Reservations;
        foreach (var reservation in RoomReservation)
        {
            var checkIn = reservation.CheckInDate;
            var checkOut = reservation.CheckOutDate;
            var diff = checkOut.Subtract(checkIn);
            var totalDiff = userDiff - diff;
            if (totalDiff == TimeSpan.Zero)
            {
                return new ResultDTO
                {
                    Message = "The reservation is occupied",
                    result = false

                };
            }
        }
    }
}