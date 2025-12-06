using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.models;

namespace server.Data;

public class ApplicationDbContext:IdentityDbContext<User>
{
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne<Room>(r => r.Room)
            .WithMany(room => room.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Token>()
            .HasOne(u => u.User)
            .WithOne(t => t.RefreshToken)
            .HasForeignKey<Token>(rt => rt.UserId)
            .OnDelete(DeleteBehavior.
                Cascade);
        modelBuilder.Entity<RoomType>()
            .HasMany(rt => rt.RoomList)
            .WithOne(r => r.Type)
            .HasForeignKey(e => e.RoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}