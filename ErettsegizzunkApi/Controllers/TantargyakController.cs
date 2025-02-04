using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.DTOs;

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

        // GET: api/Tantargyak
        [HttpGet("get-tantargy")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetTantargyak()
        {
            return await _context.Subjects.ToListAsync();
        }

        // PUT: api/Tantargyaks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("put-tantargy")]
        public async Task<IActionResult> PutTantargyak([FromBody]TantargyDTO tantargy)
        {
            _context.Entry(tantargy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TantargyakExists(tantargy.Id))
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
        /*
        // POST: api/Tantargyaks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tantargyak>> PostTantargyak(Tantargyak tantargyak)
        {
            _context.Tantargyaks.Add(tantargyak);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTantargyak", new { id = tantargyak.Id }, tantargyak);
        }*/
        /*
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
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
