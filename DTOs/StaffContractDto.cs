using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class StaffContractDto
{
    public string Id { get; set; }
    public string StaffId { get; set; } = null!;
    public string ShopId { get; set; } = null!;
    public List<OperatingHours> OperatingHours { get; set; } = new();
    public List<string> Bookables { get; set; } = new();

    public StaffContractDto(StaffContract contract)
    {
        Id = contract.Id;
        StaffId = contract.Staff.Id;
        ShopId = contract.Shop.Id;
        OperatingHours = contract.OperatingHours;
        Bookables = new();
        foreach (Bookable bookable in contract.Bookables)
        {
            Bookables.Add(bookable.Id);
        }
    }
}
