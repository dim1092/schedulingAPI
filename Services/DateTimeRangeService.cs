namespace SchedulingAPI.Services;

using SchedulingAPI.Models;
using SchedulingAPI.Services.Interfaces;

public class DateTimeRangeService
{

    DateTimeRange Range { get; }
    public DateTimeRangeService(DateTimeRange range)
    {
        Range = range;
    }

    /// <summary>
    /// Checks if two time ranges overlap each other
    /// </summary>
    /// <param name="range">An other DateTimeRange</param>
    /// <returns>True if overlap, flase otherweise</returns>
    /// </summary>
    public static bool Overlap(DateTimeRange range1, DateTimeRange range2)
    {
        // check range1 start is between range2
        if (IsBetweenEndExclusive(range1.Start, range2))
        {
            return true;
        }
        // check range1 end is between range2
        if (IsBetweenStartExclusive(range1.End, range2))
        {
            return true;
        }
        // check range2 start is between range1
        if (IsBetweenEndExclusive(range2.Start, range1))
        {
            return true;
        }
        // check range2 end is between range1
        if (IsBetweenStartExclusive(range2.End, range1))
        {
            return true;
        }

        return false;
    }

    private static bool IsBetweenEndExclusive(DateTime date, DateTimeRange range)
    {
        return date >= range.Start && date < range.End;
    }

    private static bool IsBetweenStartExclusive(DateTime date, DateTimeRange range)
    {
        return date > range.Start && date <= range.End;
    }
}
