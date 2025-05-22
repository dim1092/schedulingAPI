namespace SchedulingAPI.Models;

public class Bookable
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public List<User> JoiningUsers { get; set; } = null!;
    public string? Description { get; set; }
    public double Duration { get; set; }
    public List<OperatingHours> OperatingHours { get; set; } = null!;
}
