using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlockchainDemonstratorApi.Data;
using BlockchainDemonstratorApi.Models.Classes;

namespace BlockchainDemonstratorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorsController : ControllerBase
    {
        private readonly BeerGameContext _context;

        public FactorsController(BeerGameContext context)
        {
            _context = context;
        }

        // GET: api/Factors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factors>>> GetFactors()
        {
            return await _context.Factors.ToListAsync();
        }

        // GET: api/Factors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factors>> GetFactors(string id)
        {
            var factors = await _context.Factors.FindAsync(id);

            if (factors == null)
            {
                return NotFound();
            }

            return factors;
        }

        // PUT: api/Factors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactors(string id, Factors factors)
        {
            if (id != factors.Id)
            {
                return BadRequest();
            }

            _context.Entry(factors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactorsExists(id))
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

        // POST: api/Factors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Factors>> PostFactors(Factors factors)
        {
            _context.Factors.Add(factors);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FactorsExists(factors.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFactors", new { id = factors.Id }, factors);
        }

        // DELETE: api/Factors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Factors>> DeleteFactors(string id)
        {
            var factors = await _context.Factors.FindAsync(id);
            if (factors == null)
            {
                return NotFound();
            }

            _context.Factors.Remove(factors);
            await _context.SaveChangesAsync();

            return factors;
        }

        private bool FactorsExists(string id)
        {
            return _context.Factors.Any(e => e.Id == id);
        }
    }
}
