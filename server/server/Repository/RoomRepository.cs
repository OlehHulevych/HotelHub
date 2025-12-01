using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTO;
using server.IRepositories;
using server.models;
using server.Tools;

namespace server.Repository;

public class RoomRepository:IRoomRepository
{
    private ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ResultDTO> createRoom(RoomDTO data)
    {
        if (data == null)
        {
            return new ResultDTO
            {
                result = false,
                Message = "There is no data"
            };
        }

        List<string> photoPaths = new List<string>();
        var folderName = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Rooms/"+data.Name);
        if (!Directory.Exists(folderName))
        {
            Directory.CreateDirectory(folderName);
        }

        foreach (var photo in data.Photos)
        {
            if (photo.Length > 0)
            {
                var filePath = Path.Combine(folderName, photo.FileName);
                photoPaths.Append(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
            }
        }

        if (!photoPaths.Any())
        {
            return new ResultDTO
            {
                result = false,
                Message = "The photos are not saved"
            };
        }

        var RoomType = _context.RoomTypes.FirstOrDefaultAsync(type => type.Name == data.RoomType);

        var newRoom = new Room
        {
            Name = data.Name,
            PricePerNight = data.pricePerNight,
            Photos = photoPaths,
            RoomTypeId = RoomType.Id,
            Description = data.Description
        };
        var room = await _context.Rooms.AddAsync(newRoom);
        await _context.SaveChangesAsync();
        return new ResultDTO
        {
            result = true,
            Message = "The room is created",
            Item = room
        };


    }

    public async Task<ResultDTO> getRoom(int id)
    {
        var Room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (Room == null)
        {
            return new ResultDTO
            {
                result = false,
                Message = "The room is not found"
            };
        }

        return new ResultDTO
        {
            result = true,
            Message = "The Room is found",
            Item = Room
        };

    }

    public async Task<ResultDTO> updateRoom(UpdateRoomDTO data, int id)
    {
        /*var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
        {
            return new ResultDTO
            {
                result = false,
                Message = "The room is not found"
            };
        }*/

        throw new NotImplementedException();
    }

    public async Task<ResultDTO> deleteRoom(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
        {
            return new ResultDTO()
            {
                result = false,
                Message = "The room is not found"
            };
        }

        _context.Rooms.Remove(room);
        return new ResultDTO
        {
            result = true,
            Message = "The room was deleted"
        };
    }

    public async Task<PaginatedItems<Room>> getALlRooms(PaginationDTO pagination)
    {
        var query = _context.Rooms.AsQueryable();
        var length = await _context.Rooms.CountAsync();
        var items = await query.OrderBy(p => p.Id)
            .Skip((pagination.currentPage - 1) * 10)
            .Take(10)
            .ToListAsync();
        if (items == null)
        {
            return null;
        }

        return new PaginatedItems<Room>(items,length,pagination.currentPage);


    }
}