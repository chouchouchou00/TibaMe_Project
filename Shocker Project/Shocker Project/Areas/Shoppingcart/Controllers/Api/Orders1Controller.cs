using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shocker_Project.Models;

namespace Shocker_Project.Areas.Shoppingcart.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Orders1Controller : ControllerBase
    {
        private readonly db_a98a02_thm101team1001Context _context;

        public Orders1Controller(db_a98a02_thm101team1001Context context)
        {
            _context = context;
        }

        // GET: api/Orders1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders()
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrders(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var orders = await _context.Orders.FindAsync(id);

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }

        // PUT: api/Orders1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders(int id, Orders orders)
        {
            if (id != orders.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(orders).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
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

        // POST: api/Orders1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orders>> PostOrders(Orders orders)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'db_a98a02_thm101team1001Context.Orders'  is null.");
          }
            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrders", new { id = orders.OrderId }, orders);
        }

        // DELETE: api/Orders1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrders(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdersExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
