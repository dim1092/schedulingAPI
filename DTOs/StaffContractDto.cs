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

    public StaffContract ToStaffContract(ScheduleContext context)
    {
        StaffContract contract = new()
        {
            Id = Id,
            Staff = context.Users.Find(StaffId) ?? throw new ArgumentException("Staff not found"),
            Shop = context.Shops.Find(ShopId) ?? throw new ArgumentException("Shop not found"),
            OperatingHours = OperatingHours
        };
        
        foreach (string bookableId in Bookables)
        {
            Bookable? bookable = context.Bookables.Find(bookableId);
            if (bookable != null)
            {
                contract.Bookables.Add(bookable);
            }
        }
        
        return contract;
    }
}
