using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Models;
using SchedulingAPI.DTOs;

namespace SchedulingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            List<ScheduledEventDto> scheduledEventDtos = new List<ScheduledEventDto>();
            var events =  await  _context.ScheduledEvents
                .Include(e => e.Participants)
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

            return new ScheduledEventDto(scheduledEvent);
        }

        // PUT: api/ScheduledEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScheduledEvent(string id, ScheduledEventDto scheduledEvent)
        {
            if (id != scheduledEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(scheduledEvent.ToScheduledEvent(_context)).State = EntityState.Modified;

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
        public async Task<ActionResult<ScheduledEventDto>> PostScheduledEvent(ScheduledEventDto scheduledEvent)
        {
            _context.ScheduledEvents.Add(scheduledEvent.ToScheduledEvent(_context));
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

        // DELETE: api/ScheduledEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduledEvent(string id)
        {
            var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);
            if (scheduledEvent == null)
            {
                return NotFound();
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
}
