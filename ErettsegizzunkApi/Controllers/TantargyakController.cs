using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class TantargyakController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public TantargyakController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        // GET: api/Tantargyaks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tantargyak>>> GetTantargyaks()
        {
            return await _context.Tantargyaks.ToListAsync();
        }

        /*

        // GET: api/Tantargyaks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tantargyak>> GetTantargyak(int id)
        {
            var tantargyak = await _context.Tantargyaks.FindAsync(id);

            if (tantargyak == null)
            {
                return NotFound();
            }

            return tantargyak;
        }

        // PUT: api/Tantargyaks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTantargyak(int id, Tantargyak tantargyak)
        {
            if (id != tantargyak.Id)
            {
                return BadRequest();
            }

            _context.Entry(tantargyak).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TantargyakExists(id))
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

        // POST: api/Tantargyaks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tantargyak>> PostTantargyak(Tantargyak tantargyak)
        {
            _context.Tantargyaks.Add(tantargyak);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTantargyak", new { id = tantargyak.Id }, tantargyak);
        }

        // DELETE: api/Tantargyaks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTantargyak(int id)
        {
            var tantargyak = await _context.Tantargyaks.FindAsync(id);
            if (tantargyak == null)
            {
                return NotFound();
            }

            _context.Tantargyaks.Remove(tantargyak);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool TantargyakExists(int id)
        {
            return _context.Tantargyaks.Any(e => e.Id == id);
        }
    }
}
