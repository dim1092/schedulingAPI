namespace SchedulingAPI.Models;

public class OperatingHours
{
    public long Id { get; set; }
    public DataStructures.DayOfWeek DayOfWeek { get; set; }
    public List<DateTimeRange> Range { get; set; } = null!;
}

