using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<List<Level>>>> GetLevels()
        {
            try
            {
                return Ok(await _context.Levels.ToListAsync()); //---> kiszervezni ne egyből az adat menjen
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 140, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }
    }
}
