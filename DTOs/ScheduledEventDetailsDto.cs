using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class ScheduledEventDetailsDto
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public string OwnerId { get; set; } = null!;
    public DateTimeRange DateTimeRange { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string BookableId { get; set; } = null!;

    public ScheduledEventDetailsDto(ScheduledEvent ev)
    {
        Id = ev.Id;
        Name = ev.Name;
        OwnerId = ev.Owner.Id;
        DateTimeRange = ev.DateTimeRange;
        Location = ev.Location;
        BookableId = ev.Bookable.Id;
    }

    public ScheduledEvent ToScheduledEvent(ScheduleContext context)
    {
        ScheduledEvent ev = new()
        {
            Id = Id,
            Name = Name,
            Owner = context.Users.Find(OwnerId) ?? throw new ArgumentException("Owner not found"),
            DateTimeRange = DateTimeRange,
            Location = Location,
            Bookable = context.Bookables.Find(BookableId) ?? throw new ArgumentException("Bookable not found")
        };
        return ev;
    }
}
