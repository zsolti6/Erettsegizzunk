using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class UserStatisticsController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public UserStatisticsController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        //Felhasználó összes olyan feladat lekérérése amivel már valaha találkozott. !!!!!!!!!!!!!!Lapozós rendszert belerkani.!!!!!!!!!!
        [HttpPost("get-match-history")]
        public async Task<IActionResult> GetMatchHistory([FromBody] DeatiledStatisticsDTO filteredDeatiled)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(filteredDeatiled.Token) || Program.LoggedInUsers[filteredDeatiled.Token].Id != filteredDeatiled.UserId)
                {
                    //return Unauthorized(new ErrorDTO() { Id = 116, Message = "Hozzáférés megtagadva" });
                }

                List<FilteredTaskLessDTO> filteredTasks = new List<FilteredTaskLessDTO>();

                filteredTasks = await _context.UserStatistics
                    .Include(x => x.Task)
                    .Include(x => x.Task.Subject)
                    .Include(x => x.Task.Level)
                    .Where(x => x.UserId == filteredDeatiled.UserId)
                    .Select(x => new FilteredTaskLessDTO { Task = x.Task, UtolsoKitoltesDatum = x.FilloutDate, UtolsoSikeres = x.IsSuccessful })
                    .OrderByDescending(x => x.UtolsoKitoltesDatum)
                    .ToListAsync();

                return Ok(filteredTasks);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 117, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 118, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Visszadja a DTO-nak megfelelően egyes feladatok statisztikáit. ======>>>>>>>> kiszervezni 2 külön functionba mint a feladatszűrésben
        [HttpPost("get-statitstics-detailed")]
        public async Task<ActionResult<IEnumerable<List<FilteredTaskDTO>>>> GetStatisticsDeatiled()//hibakezelés
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                var bodyContent = await reader.ReadToEndAsync();

                List<FilteredTaskDTO> filteredTasks = new List<FilteredTaskDTO>();

                if (bodyContent.Contains("Szoveg"))
                {
                    FilteredDeatiledStatisticsDTO filteredDeatiled = JsonSerializer.Deserialize<FilteredDeatiledStatisticsDTO>(bodyContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    filteredTasks = FelatatStatisztikakSzurt(filteredDeatiled);

                    if (!Program.LoggedInUsers.ContainsKey(filteredDeatiled.Token) || Program.LoggedInUsers[filteredDeatiled.Token].Id != filteredDeatiled.UserId)
                    {
                        return Unauthorized(new ErrorDTO() { Id = 119, Message = "Hozzáférés megtagadva" });
                    }
                }
                else
                {
                    DeatiledStatisticsDTO statistics = JsonSerializer.Deserialize<DeatiledStatisticsDTO>(bodyContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    filteredTasks = FelatatStatisztikak(statistics);

                    if (!Program.LoggedInUsers.ContainsKey(statistics.Token) || Program.LoggedInUsers[statistics.Token].Id != statistics.UserId)
                    {
                        return Unauthorized(new ErrorDTO() { Id = 119, Message = "Hozzáférés megtagadva" });
                    }
                }

                return Ok(filteredTasks);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 120, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 121, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Statisztika szűrés
        private List<FilteredTaskDTO> FelatatStatisztikakSzurt(FilteredDeatiledStatisticsDTO filteredDeatiled)
        {
            return _context.UserStatistics
                .Include(x => x.Task)
                .Include(x => x.Task.Subject)
                .Include(x => x.Task.Level)
                .Include(x => x.Task.Themes)
                .Where(x => x.UserId == filteredDeatiled.UserId
                         && x.Task.SubjectId == filteredDeatiled.SubjectId
                         && x.Task.Themes.Select(y => y.Id).Contains(filteredDeatiled.ThemeId)
                         && (x.Task.Description.Contains(filteredDeatiled.Szoveg)
                         || x.Task.Text.Contains(filteredDeatiled.Szoveg)))
                .OrderBy(x => x.Id)
                .AsEnumerable()
                .GroupBy(x => x.TaskId)
                .Select(g =>
                {
                    UserStatistic lastEntry = g.OrderBy(x => x.FilloutDate).Last();

                    return new FilteredTaskDTO
                    {
                        Task = lastEntry.Task,
                        UtolsoKitoltesDatum = lastEntry.FilloutDate,
                        UtolsoSikeres = lastEntry.IsSuccessful,
                        JoRossz = new int[] { g.Count(x => x.IsSuccessful), g.Count(x => !x.IsSuccessful) }
                    };
                })
                .Skip(filteredDeatiled.Oldal * 50)
                .Take(50)
                .ToList();
        }

        //Statisztika szűrés nélkül
        private List<FilteredTaskDTO> FelatatStatisztikak(DeatiledStatisticsDTO deatiled)
        {
            return _context.UserStatistics
                .Include(x => x.Task)
                .Include(x => x.Task.Subject)
                .Include(x => x.Task.Level)
                .Include(x => x.Task.Themes)
                .Where(x => x.UserId == deatiled.UserId)
                .OrderBy(x => x.Id)
                .AsEnumerable()
                .GroupBy(x => x.TaskId)
                .Select(g =>
                {
                    UserStatistic lastEntry = g.OrderBy(x => x.FilloutDate).Last();

                    return new FilteredTaskDTO
                    {
                        Task = lastEntry.Task,
                        UtolsoKitoltesDatum = lastEntry.FilloutDate,
                        UtolsoSikeres = lastEntry.IsSuccessful,
                        JoRossz = new int[] { g.Count(x => x.IsSuccessful), g.Count(x => !x.IsSuccessful) }
                    };
                })
                .Skip(deatiled.Oldal * 50)
                .Take(50)
                .ToList();
        }

        //Visszadaja tantárgyakra lebontva összesen mennyi feladatot oldott meg 
        [HttpPost("get-taskFilloutCount")]
        public async Task<IActionResult> GetTaskFilloutCount([FromBody] GetFillingCountDTO getFillingCount)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(getFillingCount.Token) || Program.LoggedInUsers[getFillingCount.Token].Id != getFillingCount.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 122, Message = "Hozzáférés megtagadva" });
                }

                Dictionary<string, int> taskFilloutCount = new Dictionary<string, int>();
                taskFilloutCount = await _context.UserStatistics
                    .Include(x => x.Task.Subject)
                    .Where(x => x.UserId == getFillingCount.UserId)
                    .GroupBy(x => x.Task.Subject!.Name)
                    .ToDictionaryAsync(g => g.Key!, g => g.Select(x => x.TaskId).Count());

                return Ok(taskFilloutCount);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 123, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 124, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Új statisztika feltöülrése, minden feladatlap kitöltése után lefut
        [HttpPost("post-user-statistics")]
        public async Task<ActionResult<UserStatistic>> PostUserStatistic([FromBody] PostStatisticsDTO postStatistics)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(postStatistics.Token) || Program.LoggedInUsers[postStatistics.Token].Id != postStatistics.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 125, Message = "Hozzáférés megtagadva" });
                }

                DateTime date = DateTime.Now;
                foreach (int taskId in postStatistics.TaskIds.Keys)
                {
                    UserStatistic userStatistic = new UserStatistic()
                    {
                        UserId = postStatistics.UserId,
                        TaskId = taskId,
                        IsSuccessful = postStatistics.TaskIds[taskId],
                        FilloutDate = date
                    };

                    _context.UserStatistics.Add(userStatistic);
                }

                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 126, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 127, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 128, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok();
        }

        //Napra lebontva visszaadja, hogy mennyi feladatott töltött ki a user
        [HttpPost("get-filling-byDate")]
        public async Task<ActionResult<UserStatistic>> GetFillingByDate([FromBody] GetFillingCountDTO fillingByDateCount)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(fillingByDateCount.Token) || Program.LoggedInUsers[fillingByDateCount.Token].Id != fillingByDateCount.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 129, Message = "Hozzáférés megtagadva" });
                }

                Dictionary<string, int> taskFilloutCount = new Dictionary<string, int>();
                taskFilloutCount = await _context.UserStatistics
                    .Where(x => x.UserId == fillingByDateCount.UserId)
                    .GroupBy(x => x.FilloutDate.Date.ToString())
                    .ToDictionaryAsync(g => g.Key!, g => g.Count() / 15);

                return Ok(taskFilloutCount);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 130, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 131, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        //Adott user összes statisztika adatának törlése
        [HttpDelete("statisztika-reset")]
        public async Task<IActionResult> DeleteUserStatistic([FromBody] StatisticsResetDTO statisticsReset)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(statisticsReset.Token) || Program.LoggedInUsers[statisticsReset.Token].Id != statisticsReset.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 132, Message = "Hozzáférés megtagadva" });
                }

                User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == statisticsReset.UserId);

                if (user is null)
                {
                    return NotFound();
                }

                List<UserStatistic> userStatisticsDelete = new List<UserStatistic>();

                userStatisticsDelete = await _context.UserStatistics
                    .Where(x => x.UserId == statisticsReset.UserId)
                    .ToListAsync();

                _context.UserStatistics.RemoveRange(userStatisticsDelete);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 134, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 135, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 136, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok();
        }
    }
}
