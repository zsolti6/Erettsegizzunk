using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public PasswordController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        [HttpPost("jelszo-modositas")]
        public async Task<IActionResult> JelszoModositas([FromBody] JelszoModositDTO jelszoModosit)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(x => x.LoginName == jelszoModosit.LoginName && !x.GoogleUser);
                if (user != null)
                {
                    if (!Program.LoggedInUsers.ContainsKey(jelszoModosit.Token) && Program.LoggedInUsers[jelszoModosit.Token].LoginName == jelszoModosit.LoginName)
                    {
                        return Unauthorized(new ErrorDTO() { Id = 89, Message = "Hozzáférés megtagadva" });
                    }

                    if (Program.CreateSHA256(Program.CreateSHA256(jelszoModosit.OldPassword + user.Salt)) == user.Hash) //modosit??
                    {
                        user.Hash = Program.CreateSHA256(Program.CreateSHA256(jelszoModosit.NewPassword + user.Salt)); //modosit??
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();

                        return Ok("A jelszó módosítása sikeresen megtörtént.");
                    }
                    else
                    {
                        return StatusCode(201, "Hibás a régi jelszó!");//ne bad request legyen
                    }
                }
                else
                {
                    return BadRequest(new ErrorDTO() { Id = 93, Message = "Ilyen nevű felhasználó nem létezik" });
                }

            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 90, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 91, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 92, Message = "Hiba történt az adatok mentése közben" });
            }
        }

        [HttpPost("elfelejtett-jelszo")]
        public async Task<IActionResult> ElfelejtettJelszo([FromBody] string email)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Email == email && !x.GoogleUser);
                if (user != null)
                {
                    string jelszo = Program.GenerateSalt().Substring(0, 16);//64 is lehet
                    user.Hash = Program.CreateSHA256(Program.CreateSHA256(jelszo + user.Salt));
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    string body = $"<p>Az új jelszava: + {jelszo}</p>" +
                   "<img src='http://images.erettsegizzunk.nhely.hu/1715962531.84313.123565.jpg' alt='Image'/>";

                    Program.SendEmail(user.Email, "Elfelejtett jelszó", body);
                    return Ok("E-mail küldése megtörtént.");
                }
                else
                {
                    return StatusCode(210, "Nincs ilyen e-Mail cím!");
                }
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 113, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 114, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 115, Message = "Hiba történt az adatok mentése közben" });
            }
        }
    }
}
