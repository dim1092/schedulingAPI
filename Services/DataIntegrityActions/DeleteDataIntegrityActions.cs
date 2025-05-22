using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SchedulingAPI.Models;

namespace SchedulingAPI.Services.DataIntegrityActions;


/// <summary>
/// Class that contains the data integrity action. All actions should be done in this class.
/// </summary>
public class DeleteDataIntegrityActions
{
    /// <summary>
    /// Delete a user. This will remove the user from all events. And delete events he is the owner of.
    /// </summary>
    /// <param name="userService"></param>
    public void DeleteUser (UserService userService)
    {
        // Remove user from all events or delete if owner
        foreach (ScheduledEventService scheduledEvent in userService.GetEvents())
        {
            if (scheduledEvent.GetOwner().User == userService.User)
            {
                scheduledEvent.Delete();
            } 
            else
            {
                scheduledEvent.RemoveParticipant(userService.User);
            }
        }
        // Remove all events from user
        userService.GetEvents().Clear();

    }

    /// <summary>
    /// Delete a pro user. This will remove the user from all shops and all events.
    /// </summary>
    /// <param name="proUserService"></param>
    public void DeleteProUser (UserService proUserService)
    {
        DeleteUser( proUserService);

        // Delete all contract from shops and user
        foreach (StaffContractService contract in proUserService.GetContracts())
        {
            DeleteContract(contract);
        }
    }

    public void DeleteEvent(ScheduledEventService scheduledEventService)
    {
        // Remove event from all users
        foreach (UserService userService in scheduledEventService.GetParticipants())
        {
            userService.RemoveEvent(scheduledEventService.Event);
        }
        // Remove event from owner
        scheduledEventService.GetOwner().RemoveEvent(scheduledEventService.Event);
    }

    public void DeleteContract(StaffContractService staffContractService)
    {
        // Remove contract from user
        staffContractService.GetStaff().RemoveContract(staffContractService.Contract);
        // Remove contract from shop
        staffContractService.GetShop().RemoveContract(staffContractService.Contract);
    }

    public void DeleteShop(ShopService shopService)
    {
        // Remove shop from all users
        foreach (UserService proUserService in shopService.GetOwners())
        {
            proUserService.RemoveOwnedShop(shopService.Shop);
        }
        // Remove shop from all contracts
        foreach (StaffContractService staffContractService in shopService.GetContracts())
        {
            DeleteContract(staffContractService);
        }
    }
}
