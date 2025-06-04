using Microsoft.AspNetCore.Identity;

namespace SchedulingAPI.Models;

public class User : IdentityUser
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<ScheduledEvent> OwnedEvents { get; } = null!;
    public List<ScheduledEvent> ScheduledEvents { get; } = null!;
    public List<Shop> Shops { get; set; } = null!;
    public List<StaffContract> StaffContracts { get; set; } = null!;
}
