namespace SchedulingAPI.Models;

public class StaffContract
{
    public long Id { get; set; }
    public User Staff { get; set; } = null!;
    public Shop Shop { get; set; } = null!;
    public List<OperatingHours> OperatingHours { get; set; } = null!;
    public List<Bookable> Bookables { get; set; } = null!;
}
