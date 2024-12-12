using ErettsegizzunkApi.DTO;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class FeladatokController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public FeladatokController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoks([FromBody] FeladatokDTO get)//lapozósra csinálni
        {
            return await _context.Feladatoks.Include(m => m.Szint).Include(m => m.Tantargy).Include(m => m.Temas).Include(m => m.Tipus).Take(100).ToListAsync();
        }

        [HttpPost("get-random-feladatok")]//Random 15 feladat tantárgy és szint (közép felső) paraméter alapján
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoksTipusSzint([FromBody] FeladatokDTO get)
        {
            if (get.Tantargy is null || get.Szint is null)
            {
                return BadRequest("Keresési adat nem lehet null.");
            }

            List<Feladatok> randomFeladatok = await _context.Feladatoks
                .Include(m => m.Szint)
                .Include(m => m.Tantargy)
                .Include(m => m.Temas)
                .Include(m => m.Tipus)
                .Where(m => m.Tantargy.Nev == get.Tantargy && m.Szint.Nev == get.Szint)
                .OrderBy(m => EF.Functions.Random())
                .Take(15)
                .ToListAsync();

            return Ok(randomFeladatok);
        }

        // GET: erettsegizzunk/Feladatok/get-egy-feladat
        [HttpPost("get-egy-feladat")]//Egy feladat lekérése id alapján
        public async Task<ActionResult<Feladatok>> GetFeladatok(FeladatokDTO get)
        {
            if (get.Id is null)
            {
                return BadRequest("Keresési adat nem lehet null.");
            }

            Feladatok? feladat = await _context.Feladatoks
                .Include(m => m.Szint)
                .Include(m => m.Tantargy)
                .Include(m => m.Temas)
                .Include(m => m.Tipus)
                .Where(m => m.Id == get.Id)
                .FirstOrDefaultAsync();

            if (feladat == null)
            {
                return NotFound("Nincs a keresésnek megfelelő elem.");
            }

            return feladat;
        }
        /*
        // PUT: api/Feladatoks/put-egy-feladat
        [HttpPut("put-egy-feladat")]
        public async Task<IActionResult> PutFeladatok(FeladatokDTO put, Feladatok feladatok)
        {
            if (put.Id != feladatok.Id)
            {
                return BadRequest();
            }

            _context.Entry(feladatok).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeladatokExists(put.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/
        /*
        // POST: api/Feladatoks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Feladatok>> PostFeladatok(Feladatok feladatok)
        {
            _context.Feladatoks.Add(feladatok);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeladatok", new { id = feladatok.Id }, feladatok);
        }

        // DELETE: api/Feladatoks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeladatok(int id)
        {
            var feladatok = await _context.Feladatoks.FindAsync(id);
            if (feladatok == null)
            {
                return NotFound();
            }

            _context.Feladatoks.Remove(feladatok);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool FeladatokExists(int id)
        {
            return _context.Feladatoks.Any(e => e.Id == id);
        }
    }
}
