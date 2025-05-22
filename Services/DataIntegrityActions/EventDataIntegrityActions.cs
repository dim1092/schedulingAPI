namespace SchedulingAPI.Services.DataIntegrityActions;

public class EventDataIntegrityActions
{
    public bool addEventToUser(ScheduledEventService scheduledEvent, UserService user)
    {
        // Check if can add event to user
        if (!user.CanAddEvent(scheduledEvent.Event))
        {
            return false;
        }

        // Check if can add user to event
        if (!scheduledEvent.CanJoin(user.User))
        {
            return false;
        }

        // Add event to user
        user.AddEvent(scheduledEvent.Event);
        // Add user to event
        scheduledEvent.AddParticipant(user.User);

        return true;
    }
}
