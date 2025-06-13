using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class BookableDto
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OwnerId { get; set; }
    public string OfferingShopId { get; set; }
    public List<string> JoiningUsersIds { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public double Duration { get; set; }
    public List<OperatingHours> OperatingHours { get; set; } = new();

    public BookableDto(Bookable bookable)
    {
        Id = bookable.Id;
        Name = bookable.Name;
        OwnerId = bookable.Owner.Id;
        OfferingShopId = bookable.OfferingShop.Id;
        JoiningUsersIds = new List<string>();
        foreach(User joining in bookable.JoiningUsers)
        {
            JoiningUsersIds.Add(joining.Id);
        }
        Description = bookable.Description;
        OperatingHours = bookable.OperatingHours;
    }
}
