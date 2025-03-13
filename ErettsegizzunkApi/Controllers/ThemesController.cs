using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public ThemesController(ErettsegizzunkContext context)//=============>>>>>>>>>>>>>>>> Ha lesznek témák megírni!!!!!!!!
        {
            _context = context;
        }

        //Témák lekérése
        [HttpGet("get-temak")]
        public async Task<ActionResult<IEnumerable<Theme>>> GetThemes()
        {
            List<Theme> theme = new List<Theme>();
            try
            {
                theme = await _context.Themes.ToListAsync();

                if (theme is null)
                {
                    return NotFound(new ErrorDTO() { Id = 94, Message = "Az elem nem található" });
                }

                return Ok(theme);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 95, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 96, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Témák lekérése
        [HttpGet("get-temak-feladatonkent")]
        public async Task<ActionResult<IEnumerable<Theme>>> GetThemesBySubject()//hibvakezelés
        {
            Dictionary<string, Theme[]> temak = new Dictionary<string, Theme[]>();
            try
            {
                string[] subjects = await _context.Subjects.Select(x => x.Name).ToArrayAsync();

                temak = _context.Themes
                    .Include(x => x.Tasks)
                    .Select(x => new
                    {
                        SubjectName = x.Tasks.First().Subject.Name,
                        Theme = x
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.SubjectName)
                    .ToDictionary(g => g.Key!, g => g.Select(x => x.Theme).ToArray());

                if (false)
                {
                    return NotFound(new ErrorDTO() { Id = 94, Message = "Az elem nem található" });
                }

                return Ok(temak);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 95, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 96, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Téma módosítása
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheme(int id, Theme theme)
        {
            if (id != theme.Id)
            {
                return BadRequest();
            }

            _context.Entry(theme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }

        //Téma feltöltése
        [HttpPost]
        public async Task<ActionResult<Theme>> PostTheme(Theme theme)
        {
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTheme", new { id = theme.Id }, theme);
        }

        //Téma törlése
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheme(int id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            _context.Themes.Remove(theme);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
