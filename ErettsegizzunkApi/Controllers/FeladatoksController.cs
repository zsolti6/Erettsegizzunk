using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeladatoksController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public FeladatoksController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        // GET: api/Feladatoks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoks()
        {
            return await _context.Feladatoks.Include(m => m.Szint).Include(m => m.Tantargy).Include(m => m.Temas).Include(m => m.Tipus).ToListAsync();
        }

        [HttpGet("{tantargy}/{szint}")]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoksTipusSzint(string tantargy, string szint)
        {
            List<Feladatok> randomItem = await _context.Feladatoks
            .FromSql($"CALL GetFilteredRandomFeladat({tantargy}, {szint})").ToListAsync(); //.AsAsyncEnumerable
            if (randomItem == null)
            {
                return NotFound($"No items found for type: {tantargy} and difficulty: {szint}");
            }

            return Ok(randomItem);
        }





        /*
        // GET: api/Feladatoks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feladatok>> GetFeladatok(int id)
        {
            var feladatok = await _context.Feladatoks.FindAsync(id);

            if (feladatok == null)
            {
                return NotFound();
            }

            return feladatok;
        }

        // PUT: api/Feladatoks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeladatok(int id, Feladatok feladatok)
        {
            if (id != feladatok.Id)
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
                if (!FeladatokExists(id))
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
