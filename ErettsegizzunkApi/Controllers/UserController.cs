using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.DTO;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;
        public UserController(ErettsegizzunkContext context)
        {
            _context = context;
        }


        [HttpPost("get-sok-felhasznalo")]
        public async Task<IActionResult> GetFull([FromBody] LoggedUserForCheckDTO logged)
        {
            if (Program.LoggedInUsers.ContainsKey(logged.Token) && Program.LoggedInUsers[logged.Token].Permission.Level == 9)
            {

                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users
                            .Include(x => x.Permission)
                            .Include(x => x.SpacedRepetitions)
                            .Include(x => x.UserStatistics)
                            .Take(50)
                            .ToListAsync());
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message); //kevésbé részletes
                    }
                }
            }
            else
            {
                return BadRequest("Nincs jogosultságod haver!");
            }
        }

        [HttpGet("nev/{token},{loginname}")]//átírni postra
        public async Task<IActionResult> GetLoginName(string token, string loginname)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.LoginName == loginname));
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(200, ex.InnerException?.Message); //ez is egy megoldás -- részletesebb hibaüzenet
                    }
                }
            }
            else
            {
                return BadRequest("Nincs jogosultságod haver!");
            }
        }

        [HttpGet("{token},{id}")]//átírni postra
        public async Task<IActionResult> GetId(string token, int id)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.Id == id));
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(200, ex.InnerException?.Message); //ez is egy megoldás -- részletesebb hibaüzenet
                    }
                }
            }
            else
            {
                return BadRequest("Nincs jogosultságod haver!");
            }
        }

        [HttpGet("Korlevel/{token}")]//átírni postra
        public async Task<IActionResult> GetKorlevel(string token)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users.Include(x => x.Permission).Select(x => new KorlevelDTO { Email = x.Email, Name = x.LoginName, PermissionName = x.Permission.Name}).ToListAsync());
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message); //kevésbé részletes
                    }
                }
            }
            else
            {
                return BadRequest("Nincs jogosultságod haver!");
            }
        }

        [HttpDelete("delete-felhasznalok")]
        public async Task<IActionResult> Delete([FromBody] FelhasznaloTorolDTO deleteDTO)
        {
            if (Program.LoggedInUsers.ContainsKey(deleteDTO.Token) && Program.LoggedInUsers[deleteDTO.Token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        List<User> felhasznalok = await _context.Users.Where(x => deleteDTO.Ids.Contains(x.Id)).ToListAsync();
                        if (felhasznalok == null)
                        {
                            return NotFound("Nincs feladat ilyen id-vel.");
                        }
                        _context.Users.RemoveRange(felhasznalok);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(200, ex.InnerException?.Message);
                    }
                }
            }
            else
            {
                return BadRequest("Nincs jogosultságod haver!");
            }
            return Ok("Felhasználó(k) törölve!");
        }
    }
}
