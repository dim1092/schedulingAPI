using SchedulingAPI.Models;

namespace SchedulingAPI.Services;

public class InvitaionScheduledEventService : ScheduledEventService
{

    public new InvitationScheduledEvent Event => (InvitationScheduledEvent)base.Event;

    /// <summary>
    /// Constroctor for the ISchdeduleService. Replace model fields with service fields for buisness logic
    /// </summary>
    /// <param name="scheduledEvent">The IschdeduledEvent from model</param>
    public InvitaionScheduledEventService(InvitationScheduledEvent scheduledEvent) : base(scheduledEvent)
    {
    }

    /// <summary>
    /// Creates a custom event with owner, participants and date
    /// </summary>
    /// <param name="owner">Owner of the event</param>
    /// <param name="name">Name of the event</param>
    /// <returns>A CustomScheduledEvent with no participants and no date</returns>
    public static InvitationScheduledEvent CreateEvent(User owner, string name)
    {
        return new InvitationScheduledEvent { Owner = owner, Name = name }; 
    }

    /// <summary>
    /// Adds a User to the event if he is invited
    /// (can't add user if user already has an other event or if he is not invited to the event)
    /// </summary>
    /// <param name="participant"> The user to be added</param>
    /// <returns>True if the participant was added, false otherwise</returns>
    public new bool AddParticipant(User participant)
    {
        if (!IsInvited(participant))
        {
            return false;
        }
        
        Event.Participants.Add(participant);
        return true;
    }

    /// <summary>
    /// Adds a User to the invitation list
    /// </summary>
    /// <param name="participant"> the user to be added</param>
    public void AddInvited(User participant)
    {
        Event.Invited.Add(participant);
    }


    /// <summary>
    /// Remove an invited user from the invited list. Removes the user from participant list if already included.
    /// Removes the ScheduledEvent from the user
    /// </summary>
    /// <param name="user">User to be removed</param>
    public void RemoveInvitation(User user)
    {
        Event.Participants.Remove(user);
        Event.Invited.Remove(user);
        user.ScheduledEvents.Remove(Event);
    }

    /// <inheritdoc/>
    public new bool CanJoin (User user)
    {
        return IsInvited(user);
    }

    /// <summary>
    /// Check if a user is invited to the event
    /// </summary>
    /// <param name="userService"></param>
    /// <returns>True if is invited</returns>
    public bool IsInvited(User userService)
    {
        return Event.Invited.Contains(userService);
    }
}
