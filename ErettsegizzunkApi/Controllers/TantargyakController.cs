using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

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

        //Összes tantárgy lekérése
        [HttpGet("get-tantargyak")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetTantargyak()
        {
            List<Subject> subjects = new List<Subject>();
            try
            {
                subjects = await _context.Subjects.ToListAsync();

                if (subjects is null)
                {
                    return NotFound(new ErrorDTO() { Id = 44, Message = "Az elem nem található" });
                }

                return Ok(subjects);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 45, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 46, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Új tantárgy feltöltése
        [HttpPost("post-tantargy")]
        public async Task<ActionResult<Subject>> PostTantargyak([FromBody] TantargyDTO post)
        {
            if (!Program.LoggedInUsers.ContainsKey(post.Token) || Program.LoggedInUsers[post.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 47, Message = "Hozzáférés megtagadva" });
            }

            if (post is null)
            {
                return BadRequest(new ErrorDTO() { Id = 48, Message = "A feltöltendő adat nem lehet üres" });
            }

            try
            {
                Subject subject = new Subject()
                {
                    Name = post.Name
                };

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 49, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 50, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 51, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Tantárgy felvitele sikeresen megtörént");
        }

        //Tantárgyak módosítása
        [HttpPut("put-tantargy")]
        public async Task<IActionResult> PutTantargyak([FromBody] TantargyPutDTO put)
        {
            if (!Program.LoggedInUsers.ContainsKey(put.Token) || Program.LoggedInUsers[put.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 52, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                foreach (Subject subject in put.subjects)
                {
                    if (subject.Id < 1)
                    {
                        return BadRequest(new ErrorDTO() { Id = 53, Message = "Helytelen azonosító" });
                    }

                    Subject? tantargy = await _context.Subjects.FindAsync(subject.Id);

                    if (tantargy is null)
                    {
                        return NotFound(new ErrorDTO() { Id = 54, Message = "A keresett adat nem található" });
                    }

                    tantargy.Name = subject.Name;
                    _context.Entry(tantargy).State = EntityState.Modified;
                }


                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 55, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 56, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 57, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Tantárgy(ak) módosítása sikeresen megtörtént");
        }

        //Tantárgyak törlése
        [HttpDelete("delete-tantargyak")]
        public async Task<IActionResult> DeleteTantargyak([FromBody] TantargyDeleteDTO tantargyak)
        {
            if (!Program.LoggedInUsers.ContainsKey(tantargyak.Token) && Program.LoggedInUsers[tantargyak.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 58, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                _context.Subjects.RemoveRange(await _context.Subjects.Where(x => tantargyak.Ids.Contains(x.Id)).ToListAsync());
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 59, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 60, Message = "Hiba történt az adatok törlése közben" });
            }
            catch (ArgumentNullException)
            {
                return NotFound(new ErrorDTO() { Id = 61, Message = "Törlendő adat nem található" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 62, Message = "Hiba történt az adatok törlése közben" });
            }

            return Ok("A tantárgyak(k) törlése sikeresen megtörtént");
        }
    }
}
