using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("jelszo-modositas")]//body
        public async Task<IActionResult> JelszoMosositas([FromBody] JelszoModositDTO jelszoModosit)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(jelszoModosit.Token) && Program.LoggedInUsers[jelszoModosit.Token].LoginName == jelszoModosit.LoginName)
                {
                    return BadRequest(new ErrorDTO() { Id = 84, Message = "Hozzáférés megtagadva" });
                }

                User? user = _context.Users.FirstOrDefault(x => x.LoginName == jelszoModosit.LoginName);
                if (user != null)
                {
                    if (Program.CreateSHA256(Program.CreateSHA256(jelszoModosit.OldPassword + user.Salt)) == user.Hash)
                    {
                        user.Hash = Program.CreateSHA256(Program.CreateSHA256(jelszoModosit.NewPassword + user.Salt));
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
                    return BadRequest("Nincs ilyen nevű felhasználó!");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{Email}")]//body
        public async Task<IActionResult> ElfelejtettJelszo(string Email)
        {
            using (var context = new ErettsegizzunkContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Email == Email);
                    if (user != null)
                    {
                        string jelszo = Program.GenerateSalt().Substring(0, 16);//64 is lehet
                        user.Hash = Program.CreateSHA256(Program.CreateSHA256(jelszo + user.Salt));
                        context.Users.Update(user);
                        await context.SaveChangesAsync();
                        Program.SendEmail(user.Email, "Elfelejtett jelszó", "Az új jelszava: " + jelszo);
                        return Ok("E-mail küldése megtörtént.");
                    }
                    else
                    {
                        return StatusCode(210, "Nincs ilyen e-Mail cím!");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(211, ex.Message);
                }
            }
        }


    }
}
