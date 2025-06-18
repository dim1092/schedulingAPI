using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulingAPI.DTOs;
using SchedulingAPI.Models;
using SchedulingAPI.Services.DataIntegrityActions;
using System.Security.Claims;

namespace SchedulingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ScheduleContext _context;

    public UsersController(ScheduleContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        List<UserDto> userDtos = new List<UserDto>();
        var users = await _context.Users
            .Include(u => u.Shops)
            .Include(u => u.OwnedEvents)
            .Include(u => u.ScheduledEvents)
            .Include(u => u.StaffContracts)
            .ToListAsync(); 

        foreach (var user in users) {
            userDtos.Add(new UserDto(user));
        }
        return userDtos;
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        var user = await _context.Users
            .Include(u => u.Shops)
            .Include(u => u.OwnedEvents)
            .Include(u => u.ScheduledEvents)
            .Include(u => u.StaffContracts)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return new UserDto(user);
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(string id, UserDetailsDto userDto)
    {
        // Debugging: Get all claims to see what's actually there
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        if (id != userDto.Id)
        {
            return BadRequest();
        }
        // Check if current authenticated user is the same as the one being updated
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != id)
        {
            return Unauthorized();
        }

        User? dbUser = await _context.Users.FindAsync(id);
        if (dbUser == null)
        {
            return NotFound();
        }
       
        dbUser.UserName = userDto.UserName;
        dbUser.Email = userDto.Email;

        _context.Entry(dbUser).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserDetailsDto userdDetailsDto)
    {
        User newUser = new User();
        newUser.UserName = userdDetailsDto.UserName;
        newUser.Email = userdDetailsDto.Email;

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(string id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
