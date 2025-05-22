using SchedulingAPI.Models;

namespace SchedulingAPI.Services;

public class StaffContractService
{
    public StaffContract Contract { get; }

    public StaffContractService(StaffContract contract)
    {
        Contract = contract;
    }

    /// <summary>
    /// Returns the staff member of the contract contained in a ProUserService.
    /// </summary>
    /// <returns>ProUserService containing the staff of the contract</returns>
    public UserService GetStaff()
    {
        return new UserService(Contract.Staff);
    }

    /// <summary>
    /// Returns the shop of the contract contained in a ShopService.
    /// </summary>
    /// <returns>ShopService containing the shop in the contract</returns>
    public ShopService GetShop()
    {
        return new ShopService(Contract.Shop);
    }
}
