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
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoks()//limitálni 100ra MAX
        {
            return await _context.Feladatoks.Include(m => m.Szint).Include(m => m.Tantargy).Include(m => m.Temas).Include(m => m.Tipus).ToListAsync();
        }

        [HttpPost("get-random-feladatok")]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoksTipusSzint([FromBody] GetRandomFeladatok keres)
        {
            if (keres is null)
            {
                return BadRequest("Hibás keresési adatok");
            }

           /* Random rnd = new Random();
            HashSet<int> randomIds = new HashSet<int>();
            while (randomIds.Count != 4)//átírni 15-re a lesz megfelelő mennyiségű adat
            {
                randomIds.Add(rnd.Next(1,_context.Feladatoks.Where(m => m.Id > 0 && m.Tantargy.Nev == keres.Tantargy).OrderBy(m => m.Id).Last().Id));
            }
           */
            List<Feladatok> randomFeladatok = await _context.Feladatoks.Include(m => m.Szint).Include(m => m.Tantargy).Include(m => m.Temas).Include(m => m.Tipus).
                Where(m => m.Tantargy.Nev == keres.Tantargy && m.Szint.Nev == keres.Szint).OrderBy(m => Guid.NewGuid()).Take(5).ToListAsync();

            /*

            List<Feladatok> randomFeladatok = await _context.Feladatoks
            .FromSql($"CALL GetFilteredRandomFeladat({keres.Tantargy}, {keres.Szint})").ToListAsync();
            if (randomFeladatok == null)
            {
                return NotFound($"No items found for type: {keres.Tantargy} and difficulty: {keres.Szint}");
            }

            foreach (var feladat in randomFeladatok)//mükszik de nem feltétlen optimális
            {
                await _context.Entry(feladat).Reference(f => f.Szint).LoadAsync();
                await _context.Entry(feladat).Reference(f => f.Tantargy).LoadAsync();
                await _context.Entry(feladat).Reference(f => f.Tipus).LoadAsync();
            }*/

            return Ok(randomFeladatok);
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
