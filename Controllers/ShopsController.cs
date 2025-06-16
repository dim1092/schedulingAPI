using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulingAPI.DTOs;
using SchedulingAPI.Models;

namespace SchedulingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public ShopsController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/Shops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopDto>>> GetShops()
        {
            List<ShopDto> shopDtos = new List<ShopDto>();  
            var shops = await _context.Shops
                .Include(s => s.Contracts)
                .Include(s => s.Owners)
                .Include(s => s.Bookables)
                .ToListAsync();

            foreach (var shop in shops)
            {
                shopDtos.Add(new ShopDto(shop));
            }
            return shopDtos;
        }

        // GET: api/Shops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopDto>> GetShop(string id)
        {
            var shop = await _context.Shops
               .Include(s => s.Contracts)
                .Include(s => s.Owners)
                .Include(s => s.Bookables)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shop == null)
            {
                return NotFound();
            }

            return new ShopDto(shop);
        }

        // PUT: api/Shops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShop(string id, ShopDto shop)
        {
            if (id != shop.Id)
            {
                return BadRequest();
            }

            _context.Entry(shop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(id))
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

        // POST: api/Shops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShopDto>> PostShop(ShopDto shopDto) // Renamed parameter to shopDto for clarity
        {
            var shopEntity = shopDto.ToShop(_context);
            shopEntity.Id = Guid.NewGuid().ToString();
            _context.Shops.Add(shopEntity);
            await _context.SaveChangesAsync();

            var createdShopDto = new ShopDto(shopEntity);

            User? owner = _context.Users.Find("20ca1de4-24ac-4736-9196-ddf79d7f14b2"); // todo test remove


            return CreatedAtAction(
                actionName: "GetShop", // Name of the GET action to retrieve a single shop
                routeValues: new { id = createdShopDto.Id }, // Pass the newly generated ID
                value: createdShopDto // Return the ShopDto with the generated ID
            );
        }

        // DELETE: api/Shops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(string id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopExists(string id)
        {
            return _context.Shops.Any(e => e.Id == id);
        }
    }
}
