using ErettsegizzunkApi.DTO;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;
        public TokenController(ErettsegizzunkContext context)
        {
            _context = context;
        }


        // GET erettsegizzunk/<TokenController>/5
        [HttpPost("vane-token")]
        public async Task<ActionResult<Token>> VanToken([FromBody] FeladatokDeleteDTO deleteDTO)
        {
            if (deleteDTO is null)
            {
                return BadRequest();
            }

            Token? vaneToken = await _context.Tokens
                .Include(x => x.Token1 == deleteDTO.Token)
                .Include(x => x.Aktiv)
                .FirstOrDefaultAsync();

            if (vaneToken is null)
            {
                return BadRequest("Nincs ilyen token!");
            }

            return vaneToken;
        }

        // POST erettsegizzunk/<TokenController>
        [HttpPost("add-token")]
        public async Task<IActionResult> AddNewToken([FromBody] AddTokenDTO addTokenDTO)
        {
            Token tokenDTO = new Token();
            Token token = new Token()
            {
                UserId = addTokenDTO.UserId,
                Token1 = addTokenDTO.Token,
                Aktiv = true,
                Login = DateTime.Now,
                Logout = DateTime.MinValue
            };

            try
            {
                _context.Tokens.Add(token);
                await _context.SaveChangesAsync();
                tokenDTO = await _context.Tokens.OrderBy(x => x.Id).LastOrDefaultAsync(x => x == token);
                if (tokenDTO is null)
                {
                    throw new NullReferenceException();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Ok(ex.Message);
            }
            catch (Exception ex)
            {
                return Ok($"Hiba: {ex.Message}");
            }

            return Ok(tokenDTO.Id);
        }

        // PUT erettsegizzunk/<TokenController>/5
        [HttpPut("kijelentkez")]
        public async Task<IActionResult> PutToken([FromBody] ModifyToken modifyToken)
        {
            if (modifyToken.Id < 1)
            {
                BadRequest("Nincs ilyen id");
            }

            Token? token = await _context.Tokens.FindAsync(modifyToken.Id);

            if (token is null)
            {
                BadRequest("Nincs ilyen id-vel rendelkező feladat");
            }

            token.Logout = modifyToken.LogOut;
            token.Aktiv = modifyToken.Aktiv;

            _context.Entry(token).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }

            return Ok("Sikeres adatmódosítás");
        }
    }
}
