using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{token}")]//átírni postra
        public async Task<IActionResult> GetFull(string token)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users.Include(x => x.Permission).ToListAsync());
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

        [HttpGet("{token},{loginname}")]//átírni postra
        public async Task<IActionResult> GetLoginName(string token, string loginname)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.LoginNev == loginname));
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

        [HttpPost("{token}")]//átírni body-ra
        public async Task<IActionResult> Post(string token, User user)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        cx.Add(user);
                        await cx.SaveChangesAsync();
                        return Ok("Új felhasználó adatai eltárolva");
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

        [HttpPut("{token}")]//átírni body-ra
        public async Task<IActionResult> Put(string token, User user)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        cx.Update(user);
                        await cx.SaveChangesAsync();
                        return Ok("Felhasználó adatai módosítva");
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

        [HttpDelete("{token}, {id}")]//átírni body-ra
        public async Task<IActionResult> Delete(string token, int id)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        cx.Remove(new User { Id = id });
                        await cx.SaveChangesAsync();
                        return Ok("Felhasználó adatai törölve");//a törölve van nem kell futtatni
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
    }
}
