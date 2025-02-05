using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;

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
        [HttpGet("get-tantargyak")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetTantargyak()
        {
            try
            {
                return await _context.Subjects.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba történt az adatok lekérdezése közben!");
            }
        }

        [HttpPost("post-tantargy")]
        public async Task<ActionResult<Subject>> PostTantargyak([FromBody] TantargyDTO tantargyak)
        {
            if (!Program.LoggedInUsers.ContainsKey(tantargyak.Token) && Program.LoggedInUsers[tantargyak.Token].Permission.Level != 9)
            {
                return BadRequest("Nincs jogosultságod!");
            }

            try
            {
                Subject subject = new Subject()
                {
                    Id = tantargyak.Id,
                    Name = tantargyak.Name,
                };

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound("Nem található az adat az adatbázisban!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba történt az adatok feltöltése közben!");
            }

            return Ok("Erőforrás sikeresen létrehozva");
        }

        [HttpPost("put-tantargy")]
        public async Task<IActionResult> PutTantargyak([FromBody] TantargyDTO tantargy)
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

        [HttpDelete("delete-tantargyak")]
        public async Task<IActionResult> DeleteTantargyak([FromBody]TantargyDeleteDTO tantargyak)
        {
            if (!Program.LoggedInUsers.ContainsKey(tantargyak.Token) && Program.LoggedInUsers[tantargyak.Token].Permission.Level != 9)
            {
                return BadRequest("Nincs jogosultságod!");
            }

            try
            {
                _context.Subjects.RemoveRange(await _context.Subjects.Where(x => tantargyak.Ids.Contains(x.Id)).ToListAsync());
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba történt az adat törlése közben kérem próbálja újra");
            }

            return Ok("Törlés sikeresen végrehajtva");
        }

        private bool TantargyakExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
