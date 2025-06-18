using SchedulingAPI.Models;
namespace SchedulingAPI.Services.DataIntegrityActions;

public class EventDataIntegrityActions
{
    public static bool CanaddEventToUser(ScheduledEvent scheduledEvent, User user)
    {
        ScheduledEventService _scheduledEvent = new ScheduledEventService(scheduledEvent);
        UserService _user = new UserService(user);
        // Check if can add event to user
        if (!_user.CanAddEvent(_scheduledEvent.Event))
        {
            return false;
        }

        // Check if can add user to event
        if (!_scheduledEvent.CanJoin(_user.User))
        {
            return false;
        }

        // Add event to user
        _user.AddEvent(_scheduledEvent.Event);
        // Add user to event
        _scheduledEvent.AddParticipant(_user.User);

        return true;
    }
}
