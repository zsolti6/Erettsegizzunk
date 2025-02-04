using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.DTOs;

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
                return BadRequest("Nincs jogosultságod!");
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
                return BadRequest("Nincs jogosultságod!");
            }
        }

        [HttpPost("get-egy-felhasznalo")]
        public async Task<IActionResult> GetId([FromBody] GetEgyFelhasznaloDTO felhasznalo)
        {
            if (Program.LoggedInUsers.ContainsKey(felhasznalo.token) && Program.LoggedInUsers[felhasznalo.token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.Id == felhasznalo.id));
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(200, ex.InnerException?.Message); //ez is egy megoldás -- részletesebb hibaüzenet biztonsági rés lehet
                    }
                }
            }
            else
            {
                return BadRequest("Nincs jogosultságod!");
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
                return BadRequest("Nincs jogosultságod!");
            }
        }


        [HttpPut("felhasznalok-modosit")]
        public async Task<IActionResult> PutFelhasznalok([FromBody] FelhasznaloModotsitDTO modosit)
        {
            if (Program.LoggedInUsers.ContainsKey(modosit.Token) && Program.LoggedInUsers[modosit.Token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        foreach (User user in modosit.users)
                        {
                            User? userSearch = await _context.Users.FindAsync(user.Id);
                            if (userSearch is null)
                            {
                                return BadRequest("nincs id");
                            }

                            userSearch.LoginName = user.LoginName;
                            userSearch.Email = user.Email;
                            userSearch.PermissionId = user.PermissionId;
                            userSearch.Active = user.Active;
                            userSearch.ProfilePicturePath = user.ProfilePicturePath;
                            userSearch.Newsletter = user.Newsletter;
                            userSearch.SignupDate = user.SignupDate;
                            _context.Entry(userSearch).State = EntityState.Modified;
                        }

                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(200, ex.InnerException?.Message);
                    }
                }
            }
            return Ok();
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
                        _context.Users.RemoveRange(await _context.Users.Where(x => deleteDTO.Ids.Contains(x.Id)).ToListAsync());
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
                return BadRequest("Nincs jogosultságod!");
            }
            return Ok("Felhasználó(k) törölve!");
        }
    }
}
