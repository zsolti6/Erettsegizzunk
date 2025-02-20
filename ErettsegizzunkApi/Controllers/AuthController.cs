using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ErettsegizzunkApi.Services;
using LoginRequest = ErettsegizzunkApi.Models.LoginRequest;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.DTOs;
using NuGet.Common;
using NuGet.Protocol.Plugins;

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

        [HttpPost("login")]
        public async Task<IActionResult> CaptchaLogin([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.CaptchaToken))
            {
                return BadRequest(new ErrorDTO() { Id = 109, Message = "CAPTCHA kitöltése szükséges!" });
            }

            bool isCaptchaValid = await _recaptchaService.VerifyRecaptchaAsync(loginRequest.CaptchaToken);
            if (!isCaptchaValid)
            {
                return BadRequest(new ErrorDTO() { Id = 110, Message = "CAPTCHA hitelesítései hiba" });
            }

            User user = await _context.Users.FirstOrDefaultAsync(x => x.LoginName == loginRequest.Username);
            if (user == null)
            {
                return Unauthorized(new ErrorDTO() { Id = 111, Message = "Hibás név, jelszó páros" });
            }

            string hash = Program.CreateSHA256(loginRequest.Password);
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

        [HttpPost("regisztracio")]
        public async Task<IActionResult> CaptchaRegistry([FromBody] CaptchaRegisrtyDTO captchaRegisrty)//refactore error kezelés
        {
            if (string.IsNullOrEmpty(captchaRegisrty.CaptchaToken))
            {
                return BadRequest(new ErrorDTO() { Id = 109, Message = "CAPTCHA kitöltése szükséges!" });
            }

            bool isCaptchaValid = await _recaptchaService.VerifyRecaptchaAsync(captchaRegisrty.CaptchaToken);
            if (!isCaptchaValid)
            {
                return BadRequest(new ErrorDTO() { Id = 110, Message = "CAPTCHA hitelesítései hiba" });
            }

            return Ok(_registryController.Registry(captchaRegisrty.user));
        }
    }
}
