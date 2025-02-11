using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ErettsegizzunkApi.Services;
using LoginRequest = ErettsegizzunkApi.Models.LoginRequest;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;
        private readonly RecaptchaService _recaptchaService;

        public AuthController(ErettsegizzunkContext context, RecaptchaService recaptchaService)
        {
            _context = context;
            _recaptchaService = recaptchaService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.CaptchaToken))
            {
                return BadRequest(new { message = "CAPTCHA is required." });
            }

            bool isCaptchaValid = await _recaptchaService.VerifyRecaptchaAsync(loginRequest.CaptchaToken);
            if (!isCaptchaValid)
            {
                return BadRequest(new { message = "CAPTCHA validation failed." });
            }

            User user = await _context.Users.FirstOrDefaultAsync(x => x.LoginName == loginRequest.Username);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // Hash the input password with the user's salt
            string hashedPassword = HashPassword(loginRequest.Password, user.Salt);
            if (user.Hash != hashedPassword)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok("Siker");
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var combinedBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
                var hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
