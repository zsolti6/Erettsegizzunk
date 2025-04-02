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
        public async Task<ActionResult<IEnumerable<List<Theme>>>> GetThemes()
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
        public async Task<ActionResult<IEnumerable<Dictionary<string, ThemeFilteredDTO[]>>>> GetThemesBySubject()
        {
            Dictionary<string, ThemeFilteredDTO[]> temak = new Dictionary<string, ThemeFilteredDTO[]>();
            try
            {
                 temak = _context.Themes
                    .Include(x => x.Tasks)
                    .Select(x => new ThemeFilteredDTO
                    {
                        SubjectName = x.Tasks.First().Subject.Name,
                        Theme = x,
                        Count = x.Tasks.Count()
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.SubjectName)
                    .ToDictionary(g => g.Key!, g => g.Select(x => x).ToArray());

                return Ok(temak);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 142, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 143, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Téma módosítása
        [HttpPut("put-tema")]
        public async Task<IActionResult> PutTheme([FromBody] PutThemeDTO putTheme)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(putTheme.Token) || Program.LoggedInUsers[putTheme.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 144, Message = "Hozzáférés megtagadva" });
                }

                foreach (Theme item in putTheme.Themes)
                {
                    Theme? theme = await _context.Themes.FindAsync(item.Id);

                    if (theme is null)
                    {
                        return NotFound(new ErrorDTO() { Id = 145, Message = "A keresett adat nem található" });
                    }

                    theme.Name = item.Name;

                    _context.Entry(theme).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 146, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 147, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Téma módosítás sikeresen megtötént!");
        }

        //Téma feltöltése
        [HttpPost("post-tema")]
        public async Task<ActionResult<Theme>> PostTheme([FromBody] PostThemeDTO postThemeDTO)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(postThemeDTO.Token) || Program.LoggedInUsers[postThemeDTO.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 148, Message = "Hozzáférés megtagadva" });
                }

                _context.Themes.Add(postThemeDTO.Theme);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 149, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 150, Message = "Hiba történt az adatok törlése közben" });
            }

            return Ok("Téma felvitele sikeresen megtörtént");
        }
    }
}
