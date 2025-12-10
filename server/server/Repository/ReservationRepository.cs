using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTO;
using server.IRepositories;
using server.models;

namespace server.Repository;

public class ReservationRepository:IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ResultDTO> createReservation(ReservationDTO data, string id)
    {
        var userCheckOut = data.CheckOut.Date;
        var userCheckIn = data.CheckIn.Date;
        var userDiff = userCheckOut.Subtract(userCheckIn);
        Console.WriteLine(userDiff);
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
            
            if (checkOut> userCheckIn || userCheckOut<checkOut)
            {
                return new ResultDTO
                {
                    Message = "The reservation is occupied",
                    result = false

                };
            }
        }
        var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id==id);
        var totalPrice = userDiff.Days * userRoom.PricePerNight;

        Reservation newReservation = new Reservation
        {
            CheckInDate = userCheckIn,
            CheckOutDate = userCheckOut,
            Room = userRoom,
            RoomId = userRoom.Id,
            Status = Statuses.Active,
            UserId = id,
            User = user,
            TotalPrice = totalPrice
            
            
        };
        await _context.Reservations.AddAsync(newReservation);
        await _context.SaveChangesAsync();
        return new ResultDTO
        {
            result = true,
            Message = "The reservation was created"
        };
    }
}