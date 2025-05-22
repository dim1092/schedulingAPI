using SchedulinAPI.Services.Interfaces;
using SchedulingAPI.Models;
using SchedulingAPI.Services.Interfaces;

namespace SchedulingAPI.Services;

public class ShopService
{
    public Shop Shop { get; }
    
    public ShopService(Shop shop)
    {
        Shop = shop;
    }

    /// <summary>
    /// Get the list of BookableService conatining the Bookable (services, activities...) the shop provides
    /// </summary>
    /// <param name="shop">The shop providing the IBookables</param>
    /// <returns>List of IBookables</returns>
    /// </summary>
    public List<BookableService> GetBookables()
    {
       List<BookableService> bookableServices = new ();
        foreach (Bookable bookable in Shop.Bookables)
        {
            bookableServices.Add(new BookableService(bookable));
        }
        return bookableServices;
    }

    /// <summary>
    /// Add a contract to the shop.
    /// </summary>
    /// <param name="contract">The contract to be added</param>
    public void AddContract(StaffContract contract)
    {
        Shop.Contracts.Add(contract);
    }

    /// <summary>
    /// Remove a contract from the shop.
    /// </summary>
    /// <param name="contract">The contract to be removed</param>
    public void RemoveContract(StaffContract contract)
    {
        Shop.Contracts.Remove(contract);
    }

    /// <summary>
    /// Get the list of StaffContractService containing the contracts the shop has with its staff
    /// </summary>
    /// <returns>List of StaffContractService</returns>
    public List<StaffContractService> GetContracts()
    {
        List<StaffContractService> result = new();
        foreach (StaffContract contract in Shop.Contracts)
        {
            result.Add(new StaffContractService(contract));
        }
        return result;
    }

    /// <summary>
    /// Get the list of owners
    /// </summary>
    /// <returns>ProUserService owner</returns>
    public List<UserService> GetOwners()
    {
        List<UserService> owners = new();
        foreach (User owner in Shop.Owners)
        {
            owners.Add(new UserService(owner));
        }
        return owners;
    }
}
