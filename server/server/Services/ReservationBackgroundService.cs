using Microsoft.EntityFrameworkCore;
using server.Data;
using server.models;

namespace server.Services;

public class ReservationBackgroundService:BackgroundService
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _services;


    public ReservationBackgroundService(ApplicationDbContext context, IServiceProvider services)
    {
        _context = context;
        _services = services;

    }

    public async Task CheckIfExpired(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var reservations = await _context.Reservations.ToListAsync();
                foreach (var reservation in reservations)
                {
                    if (DateOnly.FromDateTime(DateTime.Today) > reservation.CheckOutDate)
                    {
                        reservation.Status = Statuses.Canceled;
                    }
                }

                await _context.SaveChangesAsync();
            }
            
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.CheckIfExpired(stoppingToken);
        await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
    }
}