using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.DTOs;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        [HttpPost]//tesztelni erősen

        public async Task<IActionResult> Registry(User user)
        {
            using (var cx = new ErettsegizzunkContext())
            {
                try
                {
                    if (cx.Users.FirstOrDefault(f => f.LoginName == user.LoginName) != null)
                    {
                        return Ok("Már létezik ilyen felhasználónév!");
                    }
                    if (cx.Users.FirstOrDefault(f => f.Email == user.Email) != null)
                    {
                        return Ok("Ezzel az e-mail címmel már regisztráltak!");
                    }
                    user.PermissionId = 1;
                    user.Active = true;//falsra kell rakni ha meg lesz az emailes cucc
                    user.Hash = Program.CreateSHA256(user.Hash);
                    await cx.Users.AddAsync(user);
                    await cx.SaveChangesAsync();

                    Program.SendEmail(user.Email, "Regisztráció", $"https://localhost:7066/erettsegizzunk/Registry?felhasznaloNev={user.LoginName}&email={user.Email}");

                    return Ok("Sikeres regisztráció. Fejezze be a regisztrációját az e-mail címére küldött link segítségével!");
                }
                catch (Exception ex)
                {
                    return StatusCode(200, ex.InnerException?.Message); //ez is egy megoldás -- részletesebb hibaüzenet
                }
            }
        }

        [HttpGet]//postra átírni

        public async Task<IActionResult> EndOfTheRegistry(string felhasznaloNev, string email)//frombody + kell dto
        {
            using (var cx = new ErettsegizzunkContext())
            {
                try
                {
                    User user = await cx.Users.FirstOrDefaultAsync(f => f.LoginName == felhasznaloNev && f.Email == email);
                    if (user == null)
                    {
                        return Ok("Sikertelen a regisztráció befejezése!");
                    }
                    else
                    {
                        user.Active = true;
                        cx.Users.Update(user);
                        await cx.SaveChangesAsync();
                        return Ok("A regisztráció befejezése sikeresen megtörtént.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(200, ex.InnerException?.Message); //ez is egy megoldás -- részletesebb hibaüzenet
                }
            }   
        }
    }
}
