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
    public class BookablesController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public BookablesController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/Bookables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookableDto>>> GetBookables()
        {
            var bookables = await _context.Bookables
                .Include(b => b.Owner)
                .Include(b => b.OfferingShop)
                .Include(b => b.JoiningUsers)
                .ToListAsync();
            List<BookableDto> bookableDtos = new List<BookableDto>();
            foreach (var bookable in bookables)
            {
                bookableDtos.Add(new BookableDto(bookable));
            }
            return bookableDtos;
        }

        // GET: api/Bookables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookableDto>> GetBookable(string id)
        {
            var bookable = await _context.Bookables
                .Include(b => b.Owner)
                .Include(b => b.OfferingShop)
                .Include(b => b.JoiningUsers)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bookable == null)
            {
                return NotFound();
            }

            return new BookableDto(bookable);
        }

        // PUT: api/Bookables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookable(string id, BookableDto bookableDto)
        {
            Bookable bookable = bookableDto.ToBookable(_context);
            if (id != bookable.Id)
            {
                return BadRequest();
            }

            

            _context.Entry(bookable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookableExists(id))
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

        // POST: api/Bookables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookableDto>> PostBookable(BookableDto bookableDto)
        {
            Bookable bookable = bookableDto.ToBookable(_context);
            _context.Bookables.Add(bookable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookableExists(bookable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBookable", new { id = bookable.Id }, bookableDto);
        }

        // DELETE: api/Bookables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookable(string id)
        {
            var bookable = await _context.Bookables.FindAsync(id);
            if (bookable == null)
            {
                return NotFound();
            }

            _context.Bookables.Remove(bookable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookableExists(string id)
        {
            return _context.Bookables.Any(e => e.Id == id);
        }
    }
}
