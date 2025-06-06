﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchedulinAPI.Models;
using System.Reflection.Emit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            .OnDelete(DeleteBehavior.Restrict); // You can't delete a User if they are the owner of any event. Prevents cascade deletion.
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
