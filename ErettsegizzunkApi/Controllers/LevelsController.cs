using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public LevelsController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        //Összes szint lekérése
        [HttpGet("get-szintek")]
        public async Task<ActionResult<IEnumerable<Level>>> GetLevels()
        {
            try
            {
                return await _context.Levels.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 140, Message = "Hiba történt az adatok lekérdezése közben" });
            }
            
        }

        //Szint módosítása név alapján ====>>> nincs rendesen megírva auto generált
        [HttpPut("put-egy-szint")]
        public async Task<IActionResult> PutLevel(int id, Level level)//DTO + frombody
        {
            if (id != level.Id)
            {
                return BadRequest();
            }

            _context.Entry(level).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }
    }
}
