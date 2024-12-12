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

        //getnek nem lehet body
        [HttpGet("get-sok-feladat")]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoks([FromBody] FeladatokGetSpecificDTO get)//lapozósra csinálni
        {
            return await _context.Feladatoks.Include(x => x.Szint).Include(x => x.Tantargy).Include(x => x.Temas).Include(x => x.Tipus).Where(x => x.Id > get.mettol - 1).Take(100).ToListAsync();//nem fog működni ha törölnek vmt...
        }

        //Random 15 feladat tantárgy és szint (közép felső) paraméter alapján
        [HttpPost("get-random-feladat")]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoksTipusSzint([FromBody] FeladatokGetSpecificDTO get)
        {
            if (get.Tantargy is null || get.Szint is null)
            {
                return BadRequest("Keresési adat nem lehet null.");
            }

            List<Feladatok> randomFeladatok = await _context.Feladatoks
                .Include(x => x.Szint)
                .Include(x => x.Tantargy)
                .Include(x => x.Temas)
                .Include(x => x.Tipus)
                .Where(x => x.Tantargy.Nev == get.Tantargy && x.Szint.Nev == get.Szint)
                .OrderBy(x => EF.Functions.Random())
                .Take(15)
                .ToListAsync();

            return Ok(randomFeladatok);
        }

        //Egy feladat lekérése id alapján
        // GET: erettsegizzunk/Feladatok/get-egy-feladat
        [HttpPost("get-egy-feladat")]
        public async Task<ActionResult<Feladatok>> GetFeladatok([FromBody] FeladatokGetSpecificDTO get)
        {
            if (get.Id is null)
            {
                return BadRequest("Keresési adat nem lehet null.");
            }

            Feladatok? feladat = await _context.Feladatoks
                .Include(x => x.Szint)
                .Include(x => x.Tantargy)
                .Include(x => x.Temas)
                .Include(x => x.Tipus)
                .Where(x => x.Id == get.Id)
                .FirstOrDefaultAsync();

            if (feladat == null)
            {
                return NotFound("Nincs a keresésnek megfelelő elem.");
            }

            return feladat;
        }

        //Egy feladat módosítása id alapján
        // PUT: api/Feladatoks/put-egy-feladat
        [HttpPut("put-egy-feladat/{id}")]
        public async Task<IActionResult> PutFeladatok(int id, [FromBody]FeladatokPutPostDTO put)
        {
            if (id < 1)
            {
                BadRequest("Nincs ilyen id");
            }

            Feladatok? feladat = await _context.Feladatoks.FindAsync(id);

            if (feladat is null)
            {
                BadRequest("Nincs ilyen id");
            }

            feladat.Leiras = put.Leiras;
            feladat.Megoldasok = put.Megoldasok;
            feladat.Helyese = put.Helyese;
            feladat.TantargyId = put.TantargyId;
            feladat.TipusId = put.TipusId;
            feladat.SzintId = put.SzintId;
            
            _context.Entry(feladat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }

            return Ok("Sikeres adatmódosítás");
        }

        //Egy feladat felvitele
        // POST: api/Feladatoks/post-egy-feladat
        [HttpPost("post-egy-feladat")]
        public async Task<ActionResult<Feladatok>> PostFeladatok(FeladatokPutPostDTO put)
        {
            Feladatok feladatok = new Feladatok
            {
                Leiras = put.Leiras,
                Megoldasok = put.Megoldasok,
                Helyese = put.Helyese,
                TantargyId = put.TantargyId,
                TipusId = put.TipusId,
                SzintId = put.SzintId
            };
            try
            {
                _context.Feladatoks.Add(feladatok);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }

            return Ok("Erőforrás sikeresen létrehozva");
        }

        //Egy feladat törlése id alapján
        // DELETE: api/Feladatoks/delete-egy-feladat
        [HttpDelete("delete-egy-feladat")]
        public async Task<IActionResult> DeleteFeladatok([FromBody]int id)
        {
            var feladatok = await _context.Feladatoks.FindAsync(id);
            if (feladatok == null)
            {
                return NotFound("Nincs feladat ilyen id-vel.");
            }

            _context.Feladatoks.Remove(feladatok);
            await _context.SaveChangesAsync();

            return Ok("Törlés sikeresen végrahajtva");
        }

        private bool FeladatokExists(int id)
        {
            return _context.Feladatoks.Any(e => e.Id == id);
        }
    }
}
