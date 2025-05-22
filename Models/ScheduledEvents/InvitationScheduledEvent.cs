namespace SchedulingAPI.Models;

public class InvitationScheduledEvent : ScheduledEvent
{
    public List<User> Invited { get; set; } = null!;
}
