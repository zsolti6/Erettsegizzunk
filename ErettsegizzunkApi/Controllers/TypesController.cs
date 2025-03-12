using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Type = ErettsegizzunkApi.Models.Type;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public TypesController(ErettsegizzunkContext context)//============>>>>>>>>>>>>>>>>>>> Megírni!!!!!!!!!!!
        {
            _context = context;
        }

        //Típusok lekérdezése
        [HttpGet("get-tipusok")]
        public async Task<ActionResult<IEnumerable<Type>>> GetTypes()
        {
            return await _context.Types.ToListAsync();
        }

        //Típus módosítása
        [HttpPut("{id}")]
        public async Task<IActionResult> PutType(int id, Type @type)
        {
            if (id != @type.Id)
            {
                return BadRequest();
            }

            _context.Entry(@type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeExists(id))
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

        //Új típus feltöltése
        [HttpPost]
        public async Task<ActionResult<Type>> PostType(Type @type)
        {
            _context.Types.Add(@type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetType", new { id = @type.Id }, @type);
        }

        //Típus törlése
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            var @type = await _context.Types.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }

            _context.Types.Remove(@type);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypeExists(int id)
        {
            return _context.Types.Any(e => e.Id == id);
        }
    }
}
