using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public PermissionsController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        // GET: api/Permissions
        [HttpPost("get-permissions")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetPermissions([FromBody] string token)
        {
            try
            {
                return await _context.Permissions.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 141, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Permossion módosítása ========>>>>>>>>> megírni
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermission(int id, Permission permission)
        {
            if (id != permission.Id)
            {
                return BadRequest();
            }

            _context.Entry(permission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }

        //Permission feltöltése ===========>>>>>>>>>>> megírni
        [HttpPost]
        public async Task<ActionResult<Permission>> PostPermission(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPermission", new { id = permission.Id }, permission);
        }

        //Permission törlése ====================>>>>>>>>>>>>>>> megírni
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
