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
    public class StaffContractsController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public StaffContractsController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/StaffContracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffContractDto>>> GetStaffContracts()
        {
            var staffContract = await _context.StaffContracts.ToListAsync();
            List<StaffContractDto> staffContractDtos = new List<StaffContractDto>();
            foreach (var contract in staffContract)
            {
                staffContractDtos.Add(new StaffContractDto(contract));
            }
            return staffContractDtos;
        }

        // GET: api/StaffContracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffContractDto>> GetStaffContract(string id)
        {
            var staffContract = await _context.StaffContracts.FindAsync(id);

            if (staffContract == null)
            {
                return NotFound();
            }

            return new StaffContractDto(staffContract);
        }

        // PUT: api/StaffContracts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaffContract(string id, StaffContractDto staffContractDto)
        {
            StaffContract staffContract = staffContractDto.ToStaffContract(_context);
            if (id != staffContract.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffContract).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffContractExists(id))
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

        // POST: api/StaffContracts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StaffContractDto>> PostStaffContract(StaffContractDto staffContractDto)
        {
            StaffContract staffContract = staffContractDto.ToStaffContract(_context);
            _context.StaffContracts.Add(staffContract);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StaffContractExists(staffContract.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStaffContract", new { id = staffContract.Id }, staffContractDto);
        }

        // DELETE: api/StaffContracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffContract(string id)
        {
            var staffContract = await _context.StaffContracts.FindAsync(id);
            if (staffContract == null)
            {
                return NotFound();
            }

            _context.StaffContracts.Remove(staffContract);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StaffContractExists(string id)
        {
            return _context.StaffContracts.Any(e => e.Id == id);
        }
    }
}
