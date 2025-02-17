using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost]
        public IActionResult Logout([FromBody]string token)
        {
            if (Program.LoggedInUsers.ContainsKey(token))
            {
                Program.LoggedInUsers.Remove(token);
                return Ok("Sikeres kijelentkezés.");
            }
            else
            {
                return Ok("Sikertelen kijelentkezés.");
            }
        }

        [HttpPost("active")]
        public IActionResult Active([FromBody] string token)
        {
            return Ok(Program.LoggedInUsers.ContainsKey(token));
        }
    }
}
