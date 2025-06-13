using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class UserDto
{
    public List<string> OwnedEventsIds { get; } = new();
    public List<string> ScheduledEventsIds { get; } = new();
    public List<string> ShopsIds { get; set; } = new();
    public List<string> StaffContractsIds { get; set; } = new();

    public UserDto(User user)
    {
        foreach (ScheduledEvent ev in user.OwnedEvents)
        {
            OwnedEventsIds.Add(ev.Id);
        }
        foreach (ScheduledEvent ev in user.ScheduledEvents)
        {
            ScheduledEventsIds.Add(ev.Id);
        }
        foreach (Shop shop in user.Shops)
        {
            ShopsIds.Add(shop.Id);
        }
        foreach (StaffContract contract in user.StaffContracts)
        {
            StaffContractsIds.Add(contract.Id);
        }
    }
}
