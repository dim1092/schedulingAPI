using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchedulinAPI.Models;
using System.Reflection.Emit;

namespace SchedulingAPI.Models;

public class ScheduleContext : IdentityDbContext<User>
{
    public ScheduleContext(DbContextOptions<ScheduleContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScheduledEvent>()
            .HasOne(e => e.Owner)
            .WithMany(u => u.OwnedEvents)
            .OnDelete(DeleteBehavior.Cascade); // or .Restrict, .Cascade based on your logic
    }


    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ScheduledEvent> ScheduledEvents { get; set; } = null!;
    //public DbSet<InvitationScheduledEvent> InvitationScheduledEvents { get; set; } = null!;
    public DbSet<Equipment> Equipment { get; set; } = null!;
    public DbSet<Shop> Shops { get; set; } = null!;
    public DbSet<StaffContract> StaffContracts { get; set; } = null!;
    public DbSet<OperatingHours> OperatingHours { get; set; } = null!;
    public DbSet<Bookable> Bookables { get; set; } = null!;
    public DbSet<InvitationBookable> InvitationBookables { get; set; } = null!;
    public DbSet<NeedsEquipmentBookable> NeedsEquipmentBookables { get; set; } = null!;
}
