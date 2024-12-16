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

        [HttpPost("get-sok-feladat")]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoks([FromBody] double mettol)
        {
            return await _context.Feladatoks
                .Include(x => x.Szint)
                .Include(x => x.Tantargy)
                .Include(x => x.Temas)
                .Include(x => x.Tipus)
                .Where(x => x.Id > mettol)
                .Take(100)
                .ToListAsync();
        }

        //Random 15 feladat tantárgy és szint (közép felső) paraméter alapján
        [HttpPost("get-random-feladatok")]
        public async Task<ActionResult<IEnumerable<Feladatok>>> GetFeladatoksTipusSzint([FromBody] FeladatokGetRandomDTO get)
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

        //-----------------Kell egy get get random feladat témára való szűrésre is------------------------------------

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
                BadRequest("Nincs ilyen id-vel rendelkező feladat");
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
        public async Task<ActionResult<Feladatok>> PostFeladat([FromBody]FeladatokPutPostDTO post)
        {
            Feladatok feladatok = new Feladatok
            {
                Leiras = post.Leiras,
                Megoldasok = post.Megoldasok,
                Helyese = post.Helyese,
                TantargyId = post.TantargyId,
                TipusId = post.TipusId,
                SzintId = post.SzintId
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

        //Több feladat felvitele
        // POST: api/Feladatoks/post-egy-feladat
        [HttpPost("post-tobb-feladat")]
        public async Task<ActionResult<Feladatok>> PostFeladatok([FromBody] List<FeladatokPutPostDTO> put)
        {

            try
            {
                foreach (FeladatokPutPostDTO feladatok in put)
                {
                    Feladatok feladat = new Feladatok
                    {
                        Leiras = feladatok.Leiras,
                        Megoldasok = feladatok.Megoldasok,
                        Helyese = feladatok.Helyese,
                        TantargyId = feladatok.TantargyId,
                        TipusId = feladatok.TipusId,
                        SzintId = feladatok.SzintId
                    };

                    _context.Feladatoks.Add(feladat);
                }
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
        [HttpDelete("delete-feladatok")]
        public async Task<IActionResult> DeleteFeladatok([FromBody] List<int> ids)
        {
            List<Feladatok> feladatok = await _context.Feladatoks.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (feladatok == null)
            {
                return NotFound("Nincs feladat ilyen id-vel.");
            }

            try
            {
                _context.Feladatoks.RemoveRange(feladatok);
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

            return Ok("Törlés sikeresen végrahajtva");
        }

        private bool FeladatokExists(int id)
        {
            return _context.Feladatoks.Any(e => e.Id == id);
        }
    }
}
