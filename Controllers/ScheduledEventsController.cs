using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulingAPI.DTOs;
using SchedulingAPI.Models;
using SchedulingAPI.Services.DataIntegrityActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchedulingAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ScheduledEventsController : ControllerBase
{
    private readonly ScheduleContext _context;

    public ScheduledEventsController(ScheduleContext context)
    {
        _context = context;
    }

    // GET: api/ScheduledEvents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduledEventDto>>> GetScheduledEvents()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<ScheduledEventDto> scheduledEventDtos = new List<ScheduledEventDto>();
        var events =  await  _context.ScheduledEvents
            .Include(e => e.Participants)
            .Where(e => e.Owner.Id == currentUserId || e.Participants.Any(p => p.Id == currentUserId)) // Check if current user is a participant or owner
            .ToListAsync();

        foreach (var scheduledEvent in events)
        {
            scheduledEventDtos.Add(new ScheduledEventDto(scheduledEvent));
        }
        return scheduledEventDtos;
    }

    // GET: api/ScheduledEvents/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduledEventDto>> GetScheduledEvent(string id)
    {
        var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);
        if (scheduledEvent == null)
        {
            return NotFound();
        }

        // Only the owner or participants can view the event
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != scheduledEvent.Owner.Id && !scheduledEvent.Participants.Any(p => p.Id == currentUserId))
        {
            return Unauthorized();
        }

        return new ScheduledEventDto(scheduledEvent);
    }

    // PUT: api/ScheduledEvents/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutScheduledEvent(string id, ScheduledEventDetailsDto scheduledEventDetails)
    {
        if (id != scheduledEventDetails.Id)
        {
            return BadRequest();
        }

        var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);
        if (scheduledEvent == null)
        {
            return NotFound();
        }

        // Only the owner can update the event
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != scheduledEvent.Owner.Id)
        {
            return Unauthorized();
        }
        scheduledEvent.Name = scheduledEventDetails.Name;
        scheduledEvent.DateTimeRange = scheduledEventDetails.DateTimeRange;
        scheduledEvent.Location = scheduledEventDetails.Location;

        _context.Entry(scheduledEvent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ScheduledEventExists(id))
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

    // POST: api/ScheduledEvents
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ScheduledEventDto>> PostScheduledEvent(ScheduledEventDetailsDto scheduledEventDetails)
    {
        ScheduledEvent scheduledEvent = scheduledEventDetails.ToScheduledEvent(_context);
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != scheduledEventDetails.OwnerId)
        {
            return Unauthorized("You can only create events for yourself.");
        }
        _context.ScheduledEvents.Add(scheduledEvent);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (ScheduledEventExists(scheduledEvent.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetScheduledEvent", new { id = scheduledEvent.Id }, scheduledEvent);
    }

    // POST /api/ScheduledEvents/{eventId}/participants
    [HttpPost("{eventId}/participants")]
    public async Task<IActionResult> AddParticipant(string eventId, [FromBody] string userId)
    {
        var scheduledEvent = await _context.ScheduledEvents
                                           .Include(se => se.Participants) // Eager load participants
                                           .FirstOrDefaultAsync(se => se.Id == eventId);

        // Only the user can add themselves as a participant
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != userId)
        {
            return Unauthorized();
        }

        if (scheduledEvent == null)
        {
            return NotFound($"Scheduled event with ID {eventId} not found.");
        }
        var userToAdd = await _context.Users.FindAsync(userId);
        if (userToAdd == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        // check if can add user to event
        if(!EventDataIntegrityActions.CanaddEventToUser(scheduledEvent, userToAdd)) {
            return Conflict($"User with ID {userId} cannot be added to event {eventId} due to data integrity rules.");
        }
        
        // Check if participant is already added
        if (scheduledEvent.Participants.Any(p => p.Id == userId))
        {
            return Conflict($"Participant with ID {userId} is already added to event {eventId}.");
        }

        scheduledEvent.Participants.Add(userToAdd);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE /api/ScheduledEvents/{eventId}/participants/{participantId}
    [HttpDelete("{eventId}/participants/{participantId}")]
    public async Task<IActionResult> RemoveParticipant(string eventId, string participantId)
    {
        var scheduledEvent = await _context.ScheduledEvents
                                           .Include(se => se.Participants)
                                           .FirstOrDefaultAsync(se => se.Id == eventId);
        if (scheduledEvent == null)
        {
            return NotFound($"Scheduled event with ID {eventId} not found.");
        }

        // Only the owner or the participant themselves can remove a participant
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != participantId && currentUserId != scheduledEvent.Owner.Id)
        {
            return Unauthorized();
        }

        var participantToRemove = scheduledEvent.Participants.FirstOrDefault(p => p.Id == participantId);
        if (participantToRemove == null)
        {
            return NotFound($"Participant with ID {participantId} not found in event {eventId}.");
        }

        scheduledEvent.Participants.Remove(participantToRemove);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/ScheduledEvents/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteScheduledEvent(string id)
    {
        var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);
        if (scheduledEvent == null)
        {
            return NotFound();
        }

        // Only the owner can delete the event
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != scheduledEvent.Owner.Id)
        {
            return Unauthorized();
        }


        _context.ScheduledEvents.Remove(scheduledEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ScheduledEventExists(string id)
    {
        return _context.ScheduledEvents.Any(e => e.Id == id);
    }
}
