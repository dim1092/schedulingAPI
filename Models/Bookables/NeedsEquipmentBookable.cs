using SchedulingAPI.Models;

namespace SchedulinAPI.Models;

public class NeedsEquipmentBookable : Bookable
{
    List<Equipment> Equipment {  get; set; } = null!;
}
