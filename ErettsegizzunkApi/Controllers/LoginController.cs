using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public LoginController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        [HttpPost("SaltRequest")]
        public async Task<IActionResult> SaltRequest([FromBody] string loginName)
        {
            try
            {
                User? response = await _context.Users.FirstOrDefaultAsync(x => x.LoginName == loginName && !x.GoogleUser);

                if (response is null)
                {
                    return BadRequest(new ErrorDTO() { Id = 34, Message = "Hibás név, jelszó páros" });
                }

                return Ok(response.Salt);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 35, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 36, Message = "Hiba történt a bejelentkezés során" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                string Hash = Program.CreateSHA256(loginDTO.TmpHash);
                User? loggeduser = await _context.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.LoginName == loginDTO.LoginName && x.Hash == Hash);

                if (loggeduser != null && loggeduser.Active)
                {
                    string token = Guid.NewGuid().ToString();
                    lock (Program.LoggedInUsers)
                    {
                        Program.LoggedInUsers.Add(token, loggeduser);
                    }
                    return Ok(new LoggedUserDTO { Id = loggeduser.Id, Name = loggeduser.LoginName, Email = loggeduser.Email, Permission = loggeduser.PermissionId, Newsletter = loggeduser.Newsletter, ProfilePicturePath = loggeduser.ProfilePicturePath, ProfilePicture = null, Token = token, GoogleUser = loggeduser.GoogleUser });
                }
                else
                {
                    return BadRequest(loggeduser is null || loggeduser.Active ? new ErrorDTO() { Id = 81, Message = "Hibás név, jelszó páros" } : new ErrorDTO() { Id = 37, Message = "Inaktív fiók" });
                }

            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 38, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 39, Message = "Hiba történt a bejelentkezés során" });
            }
        }


    }
}
