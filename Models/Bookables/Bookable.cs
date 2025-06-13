namespace SchedulingAPI.Models;

public class Bookable
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Shop OfferingShop { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public List<User> JoiningUsers { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public double Duration { get; set; }
    public List<OperatingHours> OperatingHours { get; set; } = new();
}
