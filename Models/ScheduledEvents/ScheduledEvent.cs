namespace SchedulingAPI.Models;

public class ScheduledEvent
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public User Owner { get; set; }
    public DateTimeRange DateTimeRange { get; set; } = null!;
    public string Location { get; set; } = null!;
    public List<User> Participants { get; } = null!;
    public Bookable Bookable { get; set; } = null!;
}

