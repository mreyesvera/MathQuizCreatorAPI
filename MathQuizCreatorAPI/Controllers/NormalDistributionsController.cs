using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;

namespace MathQuizCreatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NormalDistributionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NormalDistributionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/NormalDistributions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NormalDistribution>>> GetNormalDistributions()
        {
            return await _context.NormalDistributions.ToListAsync();
        }

        // GET: api/NormalDistributions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NormalDistribution>> GetNormalDistribution(Guid id)
        {
            var normalDistribution = await _context.NormalDistributions.FindAsync(id);

            if (normalDistribution == null)
            {
                return NotFound();
            }

            return normalDistribution;
        }

        // PUT: api/NormalDistributions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNormalDistribution(Guid id, NormalDistribution normalDistribution)
        {
            if (id != normalDistribution.NormalDistributionId)
            {
                return BadRequest();
            }

            _context.Entry(normalDistribution).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NormalDistributionExists(id))
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

        // POST: api/NormalDistributions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NormalDistribution>> PostNormalDistribution(NormalDistribution normalDistribution)
        {
            _context.NormalDistributions.Add(normalDistribution);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNormalDistribution", new { id = normalDistribution.NormalDistributionId }, normalDistribution);
        }

        // DELETE: api/NormalDistributions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNormalDistribution(Guid id)
        {
            var normalDistribution = await _context.NormalDistributions.FindAsync(id);
            if (normalDistribution == null)
            {
                return NotFound();
            }

            _context.NormalDistributions.Remove(normalDistribution);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NormalDistributionExists(Guid id)
        {
            return _context.NormalDistributions.Any(e => e.NormalDistributionId == id);
        }
    }
}
