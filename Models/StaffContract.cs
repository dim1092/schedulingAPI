namespace SchedulingAPI.Models;

public class StaffContract
{
    public string Id { get; set; }
    public User Staff { get; set; } = null!;
    public Shop Shop { get; set; } = null!;
    public List<OperatingHours> OperatingHours { get; set; } = new();
    public List<Bookable> Bookables { get; set; } = new();
}
