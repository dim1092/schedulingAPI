namespace SchedulingAPI.Models;

public class Equipment
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public Shop Owner { get; set; } = null!;
    public int Quantity { get; set; }
    public List<ScheduledEvent> ScheduledEvents { get; set; } = null!;
}
