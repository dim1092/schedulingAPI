using SchedulingAPI.Models;
using SchedulingAPI.Services.Interfaces;

namespace SchedulingAPI.Services;

public class UserService
{
    public User User { get; }
    public UserService(User user)
    {
        User = user;
    }

    /// <summary>
    /// Check if a specific user can be added to a specific event
    /// </summary>
    /// <param name="scheduledEvent">the event to wich the user would like to be added to</param>
    /// <returns>true if it can join fals otherwise</returns>
    /// </summary>
    public bool CanAddEvent(ScheduledEvent scheduledEvent)
    {
        // Check if user doesn't have other events at the same time
        foreach (ScheduledEvent userEvent in User.ScheduledEvents)
        {
            if (DateTimeRangeService.Overlap(scheduledEvent.DateTimeRange, userEvent.DateTimeRange))
            {
                return false;
            }
        }
        return true;
    }

    /// <inheritdoc />
    public List<DateTimeRange> GetNonAvailability(User performingUser, Bookable bookable)
    {
        List<DateTimeRange> unavailibilities = new ();
        foreach (ScheduledEvent userEvent in performingUser.ScheduledEvents)
        {
            // User has other event, can't join an other one
            if (userEvent.Bookable != bookable) 
            {
                unavailibilities.Add(userEvent.DateTimeRange);
            }
        }
        return unavailibilities;
    }

    /// <inheritdoc />
    public bool AddEvent(ScheduledEvent scheduledEvent)
    {
        if (!CanAddEvent(scheduledEvent)) 
        {
            return false;
        }
        User.ScheduledEvents.Add(scheduledEvent);
        return true;
    }

    /// <inheritdoc />
    public void RemoveEvent(ScheduledEvent ScheduledEvent)
    {
        User.ScheduledEvents.Remove(ScheduledEvent);
    }

    /// <summary>
    /// Get the user's scheduled events
    /// </summary>
    /// <param name="Bookable">the activity IBookable to check, If null all Sheduled events will be retuned</param>
    /// <param name="timeRange">Date and time start and end for witch to limit the list to, If null all Sheduled events will be retuned</param>
    /// <returns>User's envents encapsulated in a service</returns>
    public List<ScheduledEventService> GetEvents(DateTimeRange? timeRange = null, Bookable? bookable = null)
    {
        List<ScheduledEventService> result = new ();
        foreach (ScheduledEvent scheduledEvent in User.ScheduledEvents)
        {
            if (timeRange != null && DateTimeRangeService.Overlap(scheduledEvent.DateTimeRange, timeRange))
            {
                continue;
            }
            if (bookable != null && scheduledEvent.Bookable != bookable)
            {
                continue;
            }
            result.Add(new ScheduledEventService(scheduledEvent));
        }

        return result;
    }

    /// <summary>
    /// Add event to user. Data consistency is respected.
    /// </summary>
    /// <param name="ScheduledEvent">The event to be added</param>
    /// <returns>True if the event has been added successfully</returns>
    /// </summary>
    public bool AddEvent(IScheduledEventService ScheduledEvent)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete event from user and if its the owner of the event, call EventServices.rRmoveOwner.
    /// </summary>
    /// <param name="ScheduledEvent"></param>
    /// </summary>
    public void RemoveEvent(IScheduledEventService ScheduledEvent)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add owned shop to user
    /// </summary>
    /// <param name="shop">Shop the be added</param>
    public void AddOwnedShop(Shop shop)
    {
        User.Shops.Add(shop);
    }

    /// <summary>
    /// Get the list of shops owned by user
    /// </summary>
    /// <returns>List of ShopService woned by the user</returns>
    public List<ShopService> GetOwnedShop()
    {
        List<ShopService> result = new();
        foreach (Shop shop in User.Shops)
        {
            result.Add(new ShopService(shop));
        }
        return result;
    }

    /// <summary>
    /// Remove a shop from the list of owned shops
    /// </summary>
    /// <param name="shop">The shop to be removed</param>
    public void RemoveOwnedShop(Shop shop)
    {
        User.Shops.Remove(shop);
    }

    /// <summary>
    /// Add a contract to the contract list
    /// </summary>
    /// <param name="staffContract">The contract to be added</param>
    public void AddContract(StaffContract staffContract)
    {
        User.StaffContracts.Add(staffContract);
    }

   /// <summary>
   /// Get the list of the user's contracts
   /// </summary>
   /// <returns>List of StaffContractService</returns>
    public List<StaffContractService> GetContracts()
    {
        List<StaffContractService> result = new();
        foreach (StaffContract contract in User.StaffContracts)
        {
            result.Add(new StaffContractService(contract));
        }
        return result;
    }

    /// <summary>
    /// Remove a contract from the user list
    /// </summary>
    /// <param name="contract">The Contract to be removed</param>
    public void RemoveContract(StaffContract contract)
    {
        User.StaffContracts.Remove(contract);
    }
}
