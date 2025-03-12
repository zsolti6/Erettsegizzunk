using Microsoft.AspNetCore.Mvc;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase //potenciálisan belerakni, ha új bejelentkezést észlel ne engedje / kijelentkeztesse máshonnan!
    {
        //User kijelentkeztetése
        [HttpPost]
        public IActionResult Logout([FromBody] string token)
        {
            if (Program.LoggedInUsers.ContainsKey(token))
            {
                Program.LoggedInUsers.Remove(token);
                return Ok("Sikeres kijelentkezés.");
            }

            return Ok("Sikertelen kijelentkezés.");
        }

        //Frontendek elküldeni h user be van- ejelentkezve annak érdekében, hogyha kell ki tudja jelentkeztetni
        [HttpPost("active")]
        public IActionResult Active([FromBody] string token)
        {
            return Ok(Program.LoggedInUsers.ContainsKey(token));
        }
    }
}
