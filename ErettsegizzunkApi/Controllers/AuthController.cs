using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;
        private readonly RecaptchaService _recaptchaService;
        private readonly RegistryController _registryController;

        public AuthController(ErettsegizzunkContext context, RecaptchaService recaptchaService, RegistryController registryController)
        {
            _context = context;
            _recaptchaService = recaptchaService;
            _registryController = registryController;
        }

        //Webes felületi login kezelése
        [HttpPost("login")]
        public async Task<IActionResult> CaptchaLogin([FromBody] LoginRegistryRequestDTO LoginRegistryRequest)
        {
            try
            {
                IActionResult result = await CheckCaptcha(LoginRegistryRequest);

                if (result == BadRequest())
                {
                    return result;
                }

                User user = await _context.Users.FirstOrDefaultAsync(x => x.LoginName == LoginRegistryRequest.Username);
                if (user == null)
                {
                    return Unauthorized(new ErrorDTO() { Id = 111, Message = "Hibás név, jelszó páros" });
                }

                string hash = Program.CreateSHA256(LoginRegistryRequest.Password);
                if (user.Hash != hash)
                {
                    return Unauthorized(new ErrorDTO() { Id = 112, Message = "Hibás név, jelszó páros" });
                }

                string token = Guid.NewGuid().ToString();
                lock (Program.LoggedInUsers)
                {
                    Program.LoggedInUsers.Add(token, user);
                }

                return Ok(new LoggedUserDTO { Id = user.Id, Name = user.LoginName, Email = user.Email, Permission = user.PermissionId, Newsletter = user.Newsletter, ProfilePicturePath = user.ProfilePicturePath, ProfilePicture = null, Token = token, GoogleUser = user.GoogleUser });
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 137, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 138, Message = "Hiba történt a bejelentkezés során" });
            }
        }

        //Webes felületi regisztrációhoz capcha
        [HttpPost("regisztracio")]
        public async Task<IActionResult> CaptchaRegistry([FromBody] LoginRegistryRequestDTO LoginRegistryRequest)//error kezelés
        {
            IActionResult result = await CheckCaptcha(LoginRegistryRequest);

            if (result == BadRequest())
            {
                return result;
            }

            return await _registryController.Registry(LoginRegistryRequest.User);
        }

        private async Task<IActionResult> CheckCaptcha(LoginRegistryRequestDTO LoginRegistryRequest)
        {
            if (string.IsNullOrEmpty(LoginRegistryRequest.CaptchaToken))
            {
                return BadRequest(new ErrorDTO() { Id = 109, Message = "CAPTCHA kitöltése szükséges!" });
            }

            bool isCaptchaValid = await _recaptchaService.VerifyRecaptchaAsync(LoginRegistryRequest.CaptchaToken);
            if (!isCaptchaValid)
            {
                return BadRequest(new ErrorDTO() { Id = 110, Message = "CAPTCHA hitelesítései hiba" });
            }

            return NoContent();
        }
    }
}
