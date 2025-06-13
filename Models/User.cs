using Microsoft.AspNetCore.Identity;

namespace SchedulingAPI.Models;

public class User : IdentityUser
{
    public List<ScheduledEvent> OwnedEvents { get; } = new();
    public List<ScheduledEvent> ScheduledEvents { get; } = new();
    public List<Shop> Shops { get; set; } = new();
    public List<StaffContract> StaffContracts { get; set; } = new();
}
