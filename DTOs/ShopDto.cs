using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class ShopDto
{
    public string Id { get; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> OwnersIds { get; set; } = new();
    public List<string> BookablesIds { get; set; } = new();
    public List<string> ContractsIds { get; set; } = new();
    public string Address { get; set; } = string.Empty;

    public ShopDto() { }
    public ShopDto(Shop shop)
    {
        Id = shop.Id;
        Name = shop.Name;
        Address = shop.Address;
        OwnersIds = new List<string>();
        BookablesIds = new List<string>();
        ContractsIds = new List<string>();

        foreach (User owner in shop.Owners)
        {
            OwnersIds.Add(owner.Id);
        }
        foreach (Bookable bookable in shop.Bookables)
        {
            BookablesIds.Add(bookable.Id);
        }
        foreach (StaffContract contract in shop.Contracts)
        {
            ContractsIds.Add(contract.Id);
        }
    }

    public Shop ToShop(ScheduleContext context) {         
        Shop shop = new()
        {
            Id = Id,
            Name = Name,
            Address = Address
        };
        foreach (string ownerId in OwnersIds)
        {
            User? owner = context.Users.Find(ownerId);
            if (owner != null)
            {
                shop.Owners.Add(owner);
            }
        }
        foreach (string bookableId in BookablesIds)
        {
            Bookable? bookable = context.Bookables.Find(bookableId);
            if (bookable != null)
            {
                shop.Bookables.Add(bookable);
            }
        }
        foreach (string contractId in ContractsIds)
        {
            StaffContract? contract = context.StaffContracts.Find(contractId);
            if (contract != null)
            {
                shop.Contracts.Add(contract);
            }
        }
        return shop;
    }

}
