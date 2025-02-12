using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public UserController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        [HttpPost("get-sok-felhasznalo")]
        public async Task<IActionResult> GetFelhasznalok([FromBody] LoggedUserForCheckDTO logged)
        {
            if (!Program.LoggedInUsers.ContainsKey(logged.Token) || Program.LoggedInUsers[logged.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 63, Message = "Hozzáférés megtagadva" });
            }

            List<User> users = new List<User>();

            try
            {
                users = await _context.Users
                    .Include(x => x.Permission)
                    .Include(x => x.SpacedRepetitions)
                    .Include(x => x.UserStatistics)
                    .Take(50)
                    .ToListAsync();

                if (users is null)
                {
                    return NotFound(new ErrorDTO() { Id = 64, Message = "Az elem nem található" });
                }

            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 65, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 66, Message = "Hiba történt az adatok lekérdezése közben" });
            }

            return Ok(users);
        }
        /*
        [HttpGet("nev/{token},{loginname}")]//átírni postra
        public async Task<IActionResult> GetLoginName(string token, string loginname)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                using (ErettsegizzunkContext cx = new ErettsegizzunkContext())
                {
                    try
                    {
                        return Ok(await cx.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.LoginName == loginname));
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(200, ex.InnerException?.Message); //ez is egy megoldás -- részletesebb hibaüzenet
                    }
                }
            }
            else
            {
                return BadRequest("Nincs jogosultságod!");
            }
        }

        [HttpPost("get-egy-felhasznalo")]
        public async Task<IActionResult> GetId([FromBody] GetEgyFelhasznaloDTO felhasznalo)
        {
            if (!Program.LoggedInUsers.ContainsKey(felhasznalo.Token) && Program.LoggedInUsers[felhasznalo.Token].Permission.Level != 9)
            {
                return BadRequest(new ErrorDTO() { Id = 67, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                return Ok(await _context.Users.Include(x => x.Permission).FirstOrDefaultAsync(x => x.Id == felhasznalo.Id));
            }
            catch (Exception ex)
            {
                return StatusCode(200, ex.InnerException?.Message); //ez is egy megoldás -- részletesebb hibaüzenet biztonsági rés lehet
            }
        }*/

        [HttpPost("Korlevel")]
        public async Task<IActionResult> GetKorlevel([FromBody] string token)
        {
            if (!Program.LoggedInUsers.ContainsKey(token) || Program.LoggedInUsers[token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 67, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                return Ok(await _context.Users.Include(x => x.Permission).Select(x => new KorlevelDTO { Email = x.Email, Name = x.LoginName, PermissionName = x.Permission.Name }).ToListAsync());
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 68, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 69, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        [HttpPut("sajat-felhasznalo-modosit")]
        public async Task<IActionResult> PutFelhasznalo([FromBody] LoggedUser modosit)
        {
            if (!Program.LoggedInUsers.ContainsKey(modosit.Token) || Program.LoggedInUsers[modosit.Token].Id != modosit.Id)
            {
                return Unauthorized(new ErrorDTO() { Id = 84, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                User? userSearch = await _context.Users.FindAsync(modosit.Id);

                if (userSearch is null)
                {
                    return NotFound(new ErrorDTO() { Id = 85, Message = "Az elem nem található" });
                }

                if (userSearch.LoginName == modosit.Name)
                {
                    return BadRequest(new ErrorDTO() { Id = 100, Message = "Már létezik ilyen felhasználónév!" });
                }

                if (userSearch.Email == modosit.Email && _context.Users.FirstOrDefaultAsync(x => x.Email == modosit.Email).Id == modosit.Id)//elso resz kell?
                {
                    return BadRequest(new ErrorDTO() { Id = 101, Message = "Az e-mail cím már foglalt!" });
                }

                userSearch.LoginName = modosit.Name;
                userSearch.Email = modosit.Email;
                userSearch.ProfilePicturePath = modosit.ProfilePicturePath;
                userSearch.Newsletter = modosit.Newsletter;

                _context.Entry(userSearch).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok("Módosítás végrehajtva");
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 86, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new ErrorDTO() { Id = 87, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 88, Message = "Hiba történt az adatok mentése közben" });
            }
        }

        [HttpPut("felhasznalok-modosit")]
        public async Task<IActionResult> PutFelhasznalok([FromBody] FelhasznaloModotsitDTO modosit)
        {
            if (!Program.LoggedInUsers.ContainsKey(modosit.Token) || Program.LoggedInUsers[modosit.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 70, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                foreach (User user in modosit.Users)
                {
                    User? userSearch = await _context.Users.FindAsync(user.Id);

                    if (userSearch is null)
                    {
                        return NotFound(new ErrorDTO() { Id = 71, Message = "Az elem nem található" });
                    }

                    userSearch.LoginName = user.LoginName;
                    userSearch.Email = user.Email;
                    userSearch.PermissionId = user.PermissionId;
                    userSearch.Active = user.Active;
                    userSearch.ProfilePicturePath = user.ProfilePicturePath;
                    userSearch.Newsletter = user.Newsletter;
                    userSearch.SignupDate = user.SignupDate;
                    _context.Entry(userSearch).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                return Ok("Módosítás végrehajtva");
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 72, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new ErrorDTO() { Id = 73, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 74, Message = "Hiba történt az adatok mentése közben" });
            }
        }

        [HttpDelete("delete-felhasznalok")]
        public async Task<IActionResult> DeleteFelhasznalok([FromBody] FelhasznaloTorolDTO deleteDTO)
        {
            if (!Program.LoggedInUsers.ContainsKey(deleteDTO.Token) || Program.LoggedInUsers[deleteDTO.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 75, Message = "Hozzáférés megtagadva" });
            }

            try
            {

                List<User> users = await _context.Users.Where(x => deleteDTO.Ids.Contains(x.Id)).ToListAsync();

                if (users == null)
                {
                    return NotFound(new ErrorDTO() { Id = 76, Message = "Törlendő adat nem található" });
                }

                _context.Users.RemoveRange(users);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 77, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 78, Message = "Hiba történt az adatok törlése közben" });
            }
            catch (ArgumentNullException)
            {
                return NotFound(new ErrorDTO() { Id = 79, Message = "Törlendő adat nem található" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 80, Message = "Hiba történt az adatok törlése közben" });
            }

            return Ok(); //Üzenet?
        }
    }
}
