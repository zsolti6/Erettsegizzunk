using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public RegistryController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        [HttpPost("regisztracio")]
        public async Task<IActionResult> Registry([FromBody] User user)
        {
            try
            {
                if (_context.Users.FirstOrDefault(f => f.LoginName == user.LoginName) != null)
                {
                    return Ok("Már létezik ilyen felhasználónév!");
                }

                if (_context.Users.FirstOrDefault(f => f.Email == user.Email) != null)
                {
                    return Ok("Ezzel az e-mail címmel már regisztráltak!");
                }

                //user.PermissionId = 1;
                user.Active = true;//falsra kell rakni ha meg lesz az emailes cucc
                user.Hash = Program.CreateSHA256(user.Hash);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                Program.SendEmail(user.Email, "Regisztráció", $"https://localhost:7066/erettsegizzunk/Registry?felhasznaloNev={user.LoginName}&email={user.Email}");

                return Ok("Sikeres regisztráció. Fejezze be a regisztrációját az e-mail címére küldött link segítségével!");
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 40, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 41, Message = "Hiba történt a regisztráció során" });
            }
        }

        [HttpPost("regisztracio-megerostes")]
        public async Task<IActionResult> EndOfTheRegistry([FromBody] EndOfRegistryDTO endOfRegistry)//frombody + kell dto
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(f => f.LoginName == endOfRegistry.UserName && f.Email == endOfRegistry.Email);

                if (user == null)
                {
                    return Ok("Sikertelen a regisztráció befejezése!");
                }
                else
                {
                    user.Active = true;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok("A regisztráció befejezése sikeresen megtörtént.");
                }
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 42, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 43, Message = "Hiba történt a regisztráció során" });
            }
        }

        [HttpPost("googleLogin")]
        public async Task<IActionResult> LoginRegistryWithGoogle([FromBody] EndOfRegistryDTO fromGoogle)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Email == fromGoogle.Email);
            string token = Guid.NewGuid().ToString();

            if (user is null)
            {
                User newUser = new User()
                {
                    LoginName = fromGoogle.UserName,
                    Email = fromGoogle.Email,
                    Active = true,
                    Hash = string.Empty,
                    Salt = string.Empty,
                    PermissionId = 1,
                    Newsletter = false
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return Ok(new LoggedUser() { Id = _context.Users.FirstAsync(x => x.Email == newUser.Email).Id, Email = newUser.Email, Name = newUser.LoginName, Permission = newUser.PermissionId, ProfilePicturePath = newUser.ProfilePicturePath, Token = token });
            }

            return Ok(new LoggedUser() { Id = _context.Users.FirstAsync(x => x.Email == user.Email).Id, Email = user.Email, Name = user.LoginName, Permission = user.PermissionId, ProfilePicturePath = user.ProfilePicturePath, Token = token });
        }
    }
}
