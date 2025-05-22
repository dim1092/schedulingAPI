namespace SchedulingAPI.Models;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<ScheduledEvent> OwnedEvents { get; set; } = null!;
    public List<ScheduledEvent> ScheduledEvents { get; set; } = null!;
    public List<Shop> Shops { get; set; } = null!;
    public List<StaffContract> StaffContracts { get; set; } = null!;
}
