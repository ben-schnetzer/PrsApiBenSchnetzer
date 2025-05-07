using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using PrsApi.Models;

namespace PrsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineItemsController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public LineItemsController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: api/LineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItems()
        {
            return await _context.LineItems.ToListAsync();
        }

        //--------------------------------GetLineItemsForRequestMethod-------------------------------------

        [HttpGet("lines-for-req/{reqID}")]
        public async Task<ActionResult<List<LineItem>>> GetLineItemsForRequest(int reqID)
        {
            var lineItems = await _context.LineItems
                                          .Where(li => li.RequestId == reqID)
                                          .ToListAsync();

            if (!lineItems.Any())
            {
                return NotFound($"No LineItems found for RequestId: {reqID}");
            }

            return Ok(lineItems);
        }
        


        //------------------------------------------------------------------------------

        // GET: api/LineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);

            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }



        // PUT: api/LineItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Retrieve the Request, LineItems, and Product
                var request = await _context.Requests
                    .Include(r => r.LineItems)
                    .FirstOrDefaultAsync(r => r.Id == lineItem.RequestId);

                if (request != null)
                {
                    decimal total = 0;

                    foreach (var li in request.LineItems)
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == li.ProductId); // Assuming LineItem has a ProductId

                        if (product != null)
                        {
                            total += (decimal)(product.Price * li.Quantity);
                        }
                    }

                    request.Total = total;
                    _context.Entry(request).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
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

        // POST: api/LineItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem(LineItem lineItem)
        {
            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync();

            // Retrieve the associated Request
            var request = await _context.Requests
                .Include(r => r.LineItems)
                .FirstOrDefaultAsync(r => r.Id == lineItem.RequestId);

            if (request != null)
            {
                decimal total = 0;

                foreach (var li in request.LineItems)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == li.ProductId); // Fetch Product

                    if (product != null)
                    {
                        total += (decimal)(product.Price * li.Quantity); // Calculate total using Product.Price and LineItem.Quantity
                    }
                }

                request.Total = total; // Update request.Total with the new calculated value
                _context.Entry(request).State = EntityState.Modified;
                await _context.SaveChangesAsync(); // Save the updated request total
            }

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }

        // DELETE: api/LineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }

            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();

            // Retrieve the associated Request
            var request = await _context.Requests
                .Include(r => r.LineItems)
                .FirstOrDefaultAsync(r => r.Id == lineItem.RequestId);

            if (request != null)
            {
                decimal total = 0;

                foreach (var li in request.LineItems)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == li.ProductId); // Fetch Product

                    if (product != null)
                    {
                        total += (decimal)(product.Price * li.Quantity); // Calculate total using Product.Price and LineItem.Quantity
                    }
                }

                request.Total = total; // Update request.Total with the new calculated value
                _context.Entry(request).State = EntityState.Modified;
                await _context.SaveChangesAsync(); // Save the updated request total
            }

            return NoContent();
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }
    }
}
