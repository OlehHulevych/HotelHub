using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using server.Data;
using server.DTO;
using server.Helpers;
using server.IRepositories;
using server.models;
using server.Tools;

namespace server.Repository;

public class RoomRepository:IRoomRepository
{
    private readonly ApplicationDbContext _context;
    private readonly Cloudinary _cloudinary;
    

    public RoomRepository(ApplicationDbContext context,  IOptions<CloudinarySettings> config)
    {
        _context = context;
        var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
        

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
        foreach (var photo in data.Photos)
        {
            if (photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(photo.FileName, stream),
                    Folder = "HotelHub/"+data.Name

                };
                var UploadResult = new ImageUploadResult();
                UploadResult = await _cloudinary.UploadAsync(uploadParams);
                //Console.WriteLine("This is: "+UploadResult.SecureUrl);
                //Console.WriteLine("This is Error: "+UploadResult.Error.Message);
                photoPaths.Add(UploadResult.SecureUrl.AbsoluteUri);

            }
        }
        var RoomType = await _context.RoomTypes.FirstOrDefaultAsync(type => type.Name == data.RoomType);

        var newRoom = new Room
        {
            Name = data.Name,
            PricePerNight = data.pricePerNight,
            Photos = photoPaths,
            RoomTypeId = RoomType.Id,
            Description = data.Description,
            
            
        };
        await _context.Rooms.AddAsync(newRoom);
        await _context.SaveChangesAsync();
        return new ResultDTO
        {
            result = true,
            Message = "The room is created",
            
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
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
        {
            return new ResultDTO
            {
                result = false,
                Message = "The room is not found"
            };
        }

        var roomPhotos = room.Photos.ToList();
        if (data.deletedPhotos.Any())
        {
            var deletedPhotos = data.deletedPhotos;
            foreach (var deletePhoto in deletedPhotos)
            {
                if (File.Exists(deletePhoto))
                {
                    File.Delete(deletePhoto);
                }

                var updatedRoomPhotos = roomPhotos.Where(photo => photo != deletePhoto).ToList();
                roomPhotos = updatedRoomPhotos;
            }
        }

        if (data.newPhotos.Any())
        {
            var newPhotos = data.newPhotos;
            var folderName = Directory.GetCurrentDirectory() + "../Uploads/Photos/"+room.Name;
            foreach (var Photo in newPhotos)
            {
                var filePath = Path.Combine(folderName, Photo.FileName);
                roomPhotos.Append(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(stream);
                }
            }
        }

        room.Photos = roomPhotos;
        await _context.SaveChangesAsync();
        return new ResultDTO
        {
            result = true,
            Message = "The room was updated"
        };
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