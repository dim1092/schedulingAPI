using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> OwnedEventsIds { get; } = new();
    public List<string> ScheduledEventsIds { get; } = new();
    public List<string> ShopsIds { get; set; } = new();
    public List<string> StaffContractsIds { get; set; } = new();

    public UserDto() { }

    public UserDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Email = user.Email;

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

    public User ToUser(ScheduleContext context)
    {
        User user = new()
        {
            Id = string.Empty, // IdentityUser requires an Id to be set, but it will be set by the database
            UserName = string.Empty, // IdentityUser requires a UserName to be set
            Email = string.Empty // IdentityUser requires an Email to be set
        };
        
        foreach (string eventId in OwnedEventsIds)
        {
            ScheduledEvent? ev = context.ScheduledEvents.Find(eventId);
            if (ev != null)
            {
                user.OwnedEvents.Add(ev);
            }
        }
        foreach (string eventId in ScheduledEventsIds)
        {
            ScheduledEvent? ev = context.ScheduledEvents.Find(eventId);
            if (ev != null)
            {
                user.ScheduledEvents.Add(ev);
            }
        }
        foreach (string shopId in ShopsIds)
        {
            Shop? shop = context.Shops.Find(shopId);
            if (shop != null)
            {
                user.Shops.Add(shop);
            }
        }
        foreach (string contractId in StaffContractsIds)
        {
            StaffContract? contract = context.StaffContracts.Find(contractId);
            if (contract != null)
            {
                user.StaffContracts.Add(contract);
            }
        }
        return user;
    }
}
