using SchedulingAPI.Models;

namespace SchedulinAPI.Services.Interfaces;

public class BookableService
{
    public Bookable Bookable { get; }

    public BookableService(Bookable bookable)
    {
        Bookable = bookable;
    }

    /// <summary>
    /// Checks if the event can be created, and does so if it can
    /// </summary>
    /// <returns>True if the event was created, false otherwise</returns>
    public ScheduledEvent CreateEvent(DateTime dateOfEvent, User owner)
    {
        return new ScheduledEvent
        {
            Name = Bookable.Name,
            Owner = owner,
            DateTimeRange = new DateTimeRange { Start = dateOfEvent, End = dateOfEvent.AddHours(Bookable.Duration) },
            
        };
    }

}