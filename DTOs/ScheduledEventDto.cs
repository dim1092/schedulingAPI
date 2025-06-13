using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class ScheduledEventDto
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public string OwnerId { get; set; } = null!;
    public DateTimeRange DateTimeRange { get; set; } = null!;
    public string Location { get; set; } = null!;
    public List<string> ParticipantsId { get; set; } = null!;
    public string BookableId { get; set; } = null!;

    public ScheduledEventDto (ScheduledEvent ev)
    {
        Id = ev.Id;
        Name = ev.Name;
        OwnerId = ev.Owner.Id;
        DateTimeRange = ev.DateTimeRange;
        Location = ev.Location;
        ParticipantsId = new List<string>();
        foreach(User participant in ev.Participants)
        {
            ParticipantsId.Add(participant.Id);
        }
        BookableId = ev.Bookable.Id;
    }
}
