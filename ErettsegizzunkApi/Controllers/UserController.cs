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

        //50 felhasználó lekérdezése adminak =====>>>>>>>>y LAPOZÁS HIÁNYZIK
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
                return StatusCode(500, new ErrorDTO() { Id = 66, Message = "Hiba történt az adatok lekérdezése közben" });
            }

            return Ok(users);
        }

        //ez legit nem tom mi, átírni körlevél küldésére i guess
        [HttpPost("Korlevel")]
        public async Task<IActionResult> GetKorlevel([FromBody] string token)
        {
            if (!Program.LoggedInUsers.ContainsKey(token) || Program.LoggedInUsers[token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 67, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                return Ok(await _context.Users.Include(x => x.Permission).Select(x => new KorlevelDTO { Email = x.Email, Name = x.LoginName, PermissionName = x.Permission.Name }).ToListAsync());// <<<<========7 fuck is this
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 68, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 69, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //User saját felhasználó adatainak a módosítása
        [HttpPut("sajat-felhasznalo-modosit")]
        public async Task<IActionResult> PutFelhasznalo([FromBody] LoggedUserDTO modosit)
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

                User? existingUserByLogin = await _context.Users.FirstOrDefaultAsync(x => x.LoginName == modosit.Name);

                if (existingUserByLogin != null && existingUserByLogin.Id != modosit.Id)
                {
                    return BadRequest(new ErrorDTO() { Id = 100, Message = "Már létezik ilyen felhasználónév!" });
                }

                User? existingUserByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == modosit.Email);

                if (existingUserByEmail != null && existingUserByEmail.Id != modosit.Id)
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
                return StatusCode(500, new ErrorDTO() { Id = 88, Message = "Hiba történt az adatok mentése közben" });
            }
        }

        //Felhasználó(k) adatainak módosítása adminok számára
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
                return StatusCode(500, new ErrorDTO() { Id = 73, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 74, Message = "Hiba történt az adatok mentése közben" });
            }
        }

        //Felhasználó(k) törlése
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
                return StatusCode(500, new ErrorDTO() { Id = 80, Message = "Hiba történt az adatok törlése közben" });
            }

            return Ok("Felhasználó(k) törlése sikeresen megtörtént");
        }
    }
}
