using SchedulingAPI.Models;

namespace SchedulingAPI.DTOs;

public class UserDetailsDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public UserDetailsDto() { }

    public UserDetailsDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Email = user.Email;
    }
}
