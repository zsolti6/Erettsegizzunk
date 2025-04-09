using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Text.Json;
using Task = ErettsegizzunkApi.Models.Task;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class FeladatokController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;
        private readonly ThemesController _themesController;

        public FeladatokController(ErettsegizzunkContext context, ThemesController themesController)
        {
            _context = context;
            _themesController = themesController;
        }

        //50 feladat kilistázása adminoknak, lapozási funkcióval, a lekért lista utolsó eleme alapjén kérjük le a következő adatokat
        [HttpPost("get-sok-feladat")]
        public async Task<ActionResult<IEnumerable<List<Task>>>> GetFeladatoks([FromBody] int mettol)
        {
            List<Task> feladatok = new List<Task>();
            try
            {
                feladatok = await _context.Tasks
                    .Include(x => x.Level)
                    .Include(x => x.Subject)
                    .Include(x => x.Themes)
                    .Include(x => x.Type)
                    .Where(x => x.Id > mettol)
                    .Take(50)
                    .ToListAsync();

                if (feladatok is null)
                {
                    return NotFound(new ErrorDTO() { Id = 1, Message = "Az elem nem található" });
                }

            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 2, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 3, Message = "Hiba történt az adatok lekérdezése közben" });
            }

            return Ok(feladatok);
        }

        //Random 15 feladat tantárgy és szint (közép felső) paraméter alapján
        [HttpPost("get-random-feladatok")]
        public async Task<ActionResult<IEnumerable<List<Task>>>> GetFeladatoksTipusSzint()
        {
            List<Task> randomFeladatok = new List<Task>();

            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                var bodyContent = await reader.ReadToEndAsync();


                if (bodyContent.Contains("Themes"))
                {
                    TaskGetRandomFilterDTO szuresTheme = JsonSerializer.Deserialize<TaskGetRandomFilterDTO>(bodyContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (szuresTheme.Tantargy is null || szuresTheme.Szint is null || szuresTheme.Themes is null)
                    {
                        return BadRequest(new ErrorDTO() { Id = 4, Message = "A keresési feltétel nem lehet üres" });
                    }
                    randomFeladatok = await GetFeladatoksTipusSzintThemesRandom(szuresTheme);
                }
                else
                {
                    TaskGetRandomDTO randomFeladat = JsonSerializer.Deserialize<TaskGetRandomDTO>(bodyContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (randomFeladat.Tantargy is null || randomFeladat.Szint is null)
                    {
                        return BadRequest(new ErrorDTO() { Id = 4, Message = "A keresési feltétel nem lehet üres" });
                    }
                    randomFeladatok = await GetFeladatokTipusSzintRandom(randomFeladat);

                }

                if (randomFeladatok is null)
                {
                    return NotFound(new ErrorDTO() { Id = 5, Message = "A keresett adat nem található" });
                }
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 6, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 7, Message = "Hiba történt az adatok lekérdezése közben" });
            }

            return Ok(randomFeladatok);
        }

        //Ha nem szűrünk külön témára
        private async Task<List<Task>?> GetFeladatokTipusSzintRandom(TaskGetRandomDTO get)
        {
            List<Task>? randomFeladatok = await _context.Tasks
            .Include(x => x.Level)
            .Include(x => x.Subject)
            .Include(x => x.Themes)
            .Include(x => x.Type)
            .Where(x => x.Subject.Name == get.Tantargy && x.Level.Name == get.Szint)
            .OrderBy(x => EF.Functions.Random())
            .Take(15)
            .ToListAsync();

            return randomFeladatok;
        }

        //Temára szűrés estén
        private async Task<List<Task>> GetFeladatoksTipusSzintThemesRandom(TaskGetRandomFilterDTO get)
        {
            List<Task>? randomFeladatok = await _context.Tasks
            .Include(x => x.Level)
            .Include(x => x.Subject)
            .Include(x => x.Themes)
            .Include(x => x.Type)
            .Where(x => x.Subject.Name == get.Tantargy
                     && x.Level.Name == get.Szint
                     && x.Themes.Any(t => get.Themes.Contains(t.Id)))
            .OrderBy(x => EF.Functions.Random())
            .Take(15)
            .ToListAsync();

            return randomFeladatok;
        }

        //Egy feladat módosítása id alapján
        [HttpPut("put-egy-feladat")]
        public async Task<IActionResult> PutFeladatok([FromBody] TaskPutPostDTO put)
        {
            if (!Program.LoggedInUsers.ContainsKey(put.Token) || Program.LoggedInUsers[put.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 12, Message = "Hozzáférés megtagadva" });
            }

            if (put.Id < 1)
            {
                return BadRequest(new ErrorDTO() { Id = 13, Message = "Helytelen azonosító" });
            }

            Task? feladat = await _context.Tasks.FindAsync(put.Id);

            if (feladat is null)
            {
                return NotFound(new ErrorDTO() { Id = 14, Message = "A keresett adat nem található" });
            }

            try
            {
                feladat.Description = put.Leiras;
                feladat.Answers = put.Megoldasok;
                feladat.IsCorrect = put.Helyese;
                feladat.SubjectId = put.TantargyId;
                feladat.TypeId = put.TipusId;
                feladat.LevelId = put.SzintId;
                feladat.PicName = put.KepNev;
                feladat.Text = put.Szoveg;

                _context.Entry(feladat).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 15, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 16, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 17, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Feladat módosítása sikeresen megtörtént");
        }

        //Egy feladat felvitele
        [HttpPost("post-egy-feladat")]
        public async Task<IActionResult> PostFeladat([FromBody] TaskPutPostDTO post)
        {
            if (!Program.LoggedInUsers.ContainsKey(post.Token) || Program.LoggedInUsers[post.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 18, Message = "Hozzáférés megtagadva" });
            }

            if (post is null)
            {
                return BadRequest(new ErrorDTO() { Id = 19, Message = "A feltöltendő adat nem lehet üres" });
            }

            try
            {
                Task feladatok = new Task
                {
                    Description = post.Leiras,
                    Answers = post.Megoldasok,
                    IsCorrect = post.Helyese,
                    SubjectId = post.TantargyId,
                    TypeId = post.TipusId,
                    LevelId = post.SzintId,
                    PicName = post.KepNev
                };

                _context.Tasks.Add(feladatok);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 20, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 21, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 22, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok();
        }

        //Több feladat felvitele
        [HttpPost("post-tobb-feladat")]
        public async Task<IActionResult> PostFeladatok([FromBody] List<TaskPutPostDTO> post)
        {
            if (!Program.LoggedInUsers.ContainsKey(post[0].Token) || Program.LoggedInUsers[post[0].Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 23, Message = "Hozzáférés megtagadva" });
            }

            if (post is null)
            {
                return BadRequest(new ErrorDTO() { Id = 24, Message = "A feltöltendő adat nem lehet üres" });
            }

            try
            {
                List<Theme> temak = new List<Theme>();
                ActionResult temakResult = (await _themesController.GetThemes()).Result;

                if (temakResult is OkObjectResult asd)
                {
                    temak = (List<Theme>)asd.Value;
                }

                foreach (TaskPutPostDTO feladatok in post)
                {
                    Task feladat = new Task
                    {
                        Description = feladatok.Leiras,
                        Text = feladatok.Szoveg,
                        Answers = feladatok.Megoldasok,
                        IsCorrect = feladatok.Helyese,
                        SubjectId = feladatok.TantargyId,
                        TypeId = feladatok.TipusId,
                        LevelId = feladatok.SzintId,
                        PicName = feladatok.KepNev,
                        Themes = temak.Where(x => feladatok.Temak.Contains(x.Id.ToString())).ToList()
                    };

                    _context.Tasks.Add(feladat);
                }
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 25, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 26, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Id = 27, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Adatok feltöltése megtörtént");
        }

        //Egy vagy több feladat törlése id alapján
        [HttpDelete("delete-feladatok")]
        public async Task<IActionResult> DeleteFeladatok([FromBody] ParentDeleteDTO feladatokDeleteDTO)
        {
            if (!Program.LoggedInUsers.ContainsKey(feladatokDeleteDTO.Token) || Program.LoggedInUsers[feladatokDeleteDTO.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 28, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                List<Task> feladatok = await _context.Tasks.Where(x => feladatokDeleteDTO.Ids.Contains(x.Id)).ToListAsync();

                if (feladatok == null)
                {
                    return NotFound(new ErrorDTO() { Id = 29, Message = "Törlendő adat nem található" });
                }

                _context.Tasks.RemoveRange(feladatok);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 30, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 31, Message = "Hiba történt az adatok törlése közben" });
            }
            catch (ArgumentNullException)
            {
                return NotFound(new ErrorDTO() { Id = 32, Message = "Törlendő adat nem található" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDTO() { Id = 33, Message = "Hiba történt az adatok törlése közben" });
            }

            return Ok("Adatok törlése sikeresen megtörént");
        }
    }
}
