using SchedulingAPI.Models;
using SchedulingAPI.Services.Interfaces;

namespace SchedulingAPI.Services;

public class ScheduledEventService : IScheduledEventService
{
    public ScheduledEvent Event { get; }

    public ScheduledEventService(ScheduledEvent scheduledEvent)
    {
        Event = scheduledEvent;
    }

    /// <inheritdoc/>
    public bool CanJoin(User userService)
    {
        return true;
    }

    /// <inheritdoc/>
    public void Delete()
    {
        // Remove event from participants
        foreach (User participant in Event.Participants)
        {
            participant.ScheduledEvents.Remove(Event);
        }
        // Remove event from Owner
        Event.Owner.ScheduledEvents.Remove(Event);
    }

    /// <inheritdoc/>
    public void RemoveParticipant(User user)
    {
        Event.Participants.Remove(user);
    }

    /// <inheritdoc/>
    public void AddParticipant(User user)
    {
        Event.Participants.Add(user);
    }


    /// <summary>
    /// Get a list of participants of the event.
    /// </summary>
    /// <returns>List of UserServices</returns>
    public List<UserService> GetParticipants()
    {
        List<UserService> participants = new();
        foreach (User participant in Event.Participants)
        {
            participants.Add(new UserService(participant));
        }
        return participants;
    }

    /// <summary>
    /// Get Event owner
    /// </summary>
    /// <returns>ProUserService owner of the event</returns>
    public UserService GetOwner()
    {
        return new UserService(Event.Owner);
    }
}
