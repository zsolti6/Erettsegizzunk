using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.DTOs;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public ThemesController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        // GET: api/Themes
        [HttpGet("get-temak")]
        public async Task<ActionResult<IEnumerable<Theme>>> GetThemes()
        {
            List<Theme> subjects = new List<Theme>();
            try
            {
                subjects = await _context.Themes.ToListAsync();

                if (subjects is null)
                {
                    return NotFound(new ErrorDTO() { Id = 94, Message = "Az elem nem található" });
                }

                return Ok(subjects);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 95, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 96, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        // GET: api/Themes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theme>> GetTheme(int id)
        {
            var theme = await _context.Themes.FindAsync(id);

            if (theme == null)
            {
                return NotFound();
            }

            return theme;
        }

        // PUT: api/Themes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                if (!ThemeExists(id))
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

        // POST: api/Themes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Theme>> PostTheme(Theme theme)
        {
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTheme", new { id = theme.Id }, theme);
        }

        // DELETE: api/Themes/5
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

        private bool ThemeExists(int id)
        {
            return _context.Themes.Any(e => e.Id == id);
        }
    }
}
