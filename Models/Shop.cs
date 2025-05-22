namespace SchedulingAPI.Models;

public class Shop
{
    public long Id { get; set; }
    public String Name { get; set; } = null!;
    public List<User> Owners { get; set; } = null!;
    public List<Bookable> Bookables { get; set; } = null!;
    public List<StaffContract> Contracts { get; set; } = null!;
    public String? Address { get; set; }
}
