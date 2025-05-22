namespace SchedulingAPI.Services.Interfaces;

using SchedulingAPI.Models;

public interface IScheduledEventService
{
    /// <summary>
    /// Checks if a user can join the event
    /// </summary>
    /// <param name="userService">User service that contains the user to be added</param>
    /// <returns>true if the user can be added, false otherwise</returns>
    /// </summary>
    public bool CanJoin(User userService);

    /// <summary>
    /// Adds a participant to the event
    /// </summary>
    /// <param name="userService">The User to be added</param>
    public void AddParticipant(User userService);

    /// <summary>
    /// Removes a participant from the event
    /// </summary>
    /// <param name="user">User to remove</param>
    public void RemoveParticipant(User user);
}
