namespace SchedulingAPI.Models;

public class Shop
{
    public string Id { get; set; } = null!;
    public String Name { get; set; } = "";
    public List<User> Owners { get; set; } = new();
    public List<Bookable> Bookables { get; set; } = new();
    public List<StaffContract> Contracts { get; set; } = new();
    public String Address { get; set; } = "";
}
