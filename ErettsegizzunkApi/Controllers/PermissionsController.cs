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
        public async Task<ActionResult<IEnumerable<List<Permission>>>> GetPermissions([FromBody] string token)
        {
            try
            {
                return Ok(await _context.Permissions.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 141, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Permission módosítása
        [HttpPut("put-permissions")]
        public async Task<IActionResult> PutPermission([FromBody] PutPostPermissionDTO putPermission)//----> Hibakezelés
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(putPermission.Token) || Program.LoggedInUsers[putPermission.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 12, Message = "Hozzáférés megtagadva" });
                }

                Permission? permission = await _context.Permissions.FindAsync(putPermission.Permission.Id);

                if (permission is null)
                {
                    return NotFound(new ErrorDTO() { Id = 14, Message = "A keresett adat nem található" });
                }

                permission.Name = putPermission.Permission.Name;
                permission.Description = putPermission.Permission.Description;
                permission.Level = putPermission.Permission.Level;

                _context.Entry(permission).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        //Permission feltöltése
        [HttpPost("post-permissions")]
        public async Task<IActionResult> PostPermission([FromBody] PutPostPermissionDTO postPermission)//---> hibakezelés
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(postPermission.Token) || Program.LoggedInUsers[postPermission.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 12, Message = "Hozzáférés megtagadva" });
                }

                _context.Permissions.Add(postPermission.Permission);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        //Permission törlése
        [HttpDelete("delete-permission")]
        public async Task<IActionResult> DeletePermission([FromBody] ParentDeleteDTO deletePermission)//----> hibakezelés
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(deletePermission.Token) || Program.LoggedInUsers[deletePermission.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 12, Message = "Hozzáférés megtagadva" });
                }

                foreach (int id in deletePermission.Ids)
                {
                    Permission permission = await _context.Permissions.FindAsync(id);
                    if (permission == null)
                    {
                        return NotFound();
                    }

                    _context.Permissions.Remove(permission);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                StatusCode(500);
            }

            return Ok();
        }
    }
}
