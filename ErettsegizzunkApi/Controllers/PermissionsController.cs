using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

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

        //Permission lekérése
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
        public async Task<IActionResult> PutPermission([FromBody] PutPermissionDTO putPermission)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(putPermission.Token) || Program.LoggedInUsers[putPermission.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 151, Message = "Hozzáférés megtagadva" });
                }

                foreach (Permission item in putPermission.Permissions)
                {
                    Permission? permission = await _context.Permissions.FindAsync(item.Id);

                    if (permission is null)
                    {
                        return NotFound(new ErrorDTO() { Id = 152, Message = "A keresett adat nem található" });
                    }

                    permission.Name = item.Name;
                    permission.Description = item.Description;
                    permission.Level = item.Level;

                    _context.Entry(permission).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 153, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 154, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Engedély(ek) módosítása sikeresen megtörtént");
        }

        //Permission feltöltése
        [HttpPost("post-permissions")]
        public async Task<IActionResult> PostPermission([FromBody] PostPermissionDTO postPermission)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(postPermission.Token) || Program.LoggedInUsers[postPermission.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 155, Message = "Hozzáférés megtagadva" });
                }

                _context.Permissions.Add(postPermission.Permission);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 156, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 157, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Engedély felvitele sikeresen megtörtént");
        }

        //Permission törlése
        [HttpDelete("delete-permission")]
        public async Task<IActionResult> DeletePermission([FromBody] ParentDeleteDTO deletePermission)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(deletePermission.Token) || Program.LoggedInUsers[deletePermission.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 158, Message = "Hozzáférés megtagadva" });
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
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 159, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 160, Message = "Hiba történt az adatok törlése közben" });
            }

            return Ok("Engedély(ek) törlése sikeresen megtörtént");
        }
    }
}
