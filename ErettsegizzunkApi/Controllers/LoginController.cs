using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("SaltRequest")]
        public async Task<IActionResult> SaltRequest([FromBody]string loginName)
        {
            using (var cx = new ErettsegizzunkContext())
            {
                try
                {
                    User response = await cx.Users.FirstOrDefaultAsync(x => x.LoginNev == loginName);
                    if (response is null)
                    {
                        return BadRequest("Hibavanbazzeg");
                    }
                    return Ok(response.Salt);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.InnerException?.Message);
                }
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            using (var cx = new ErettsegizzunkContext())
            {
                try
                {
                    string Hash = Program.CreateSHA256(loginDTO.TmpHash);
                    User loggeduser =  await cx.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.LoginNev == loginDTO.LoginName && x.Hash == Hash);

                    if (loggeduser != null && loggeduser.Active)
                    {
                        string token = Guid.NewGuid().ToString();
                        lock (Program.LoggedInUsers)
                        {
                            Program.LoggedInUsers.Add(token, loggeduser);
                        }                        
                        return Ok(new LoggedUser { Name = loggeduser.Name, Email = loggeduser.Email, Permission = loggeduser.PermissionId, ProfilePicturePath = loggeduser.ProfilePicturePath, Token = token});
                    }
                    else
                    {
                        return BadRequest(new LoggedUser { Permission = -1, Name = "Hibás név, jelszó páros!"});
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(new LoggedUser { Permission = -1, Name = ex.Message});
                }
            }
        }
    }
}
