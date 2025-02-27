using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Task = System.Threading.Tasks.Task;

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

        [HttpPost("get-all-statistics")]
        public async Task<ActionResult<IEnumerable<UserStatistic>>> GetUserStatistics([FromBody] GetAllStatisticsDTO getAllStatistics)
        {
            List<UserStatistic> statistics = new List<UserStatistic>();

            try
            {
                if (!Program.LoggedInUsers.ContainsKey(getAllStatistics.Token) && Program.LoggedInUsers[getAllStatistics.Token].PermissionId != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 126, Message = "Hozzáférés megtagadva" });
                }

                statistics = await _context.UserStatistics
                    .Where(x => x.Id > getAllStatistics.Mettol)
                    .Take(50)
                    .ToListAsync();

                if (statistics is null)
                {
                    return NotFound(new ErrorDTO() { Id = 129, Message = "Az elem nem található" });
                }

            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 127, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 128, Message = "Hiba történt az adatok lekérdezése közben" });
            }

            return Ok(statistics);
        }

        [HttpPost("get-one-statistics")]
        public async Task<ActionResult<UserStatistic>> GetUserStatistic([FromBody] GetOneStatisticsDTO getOneStatistics)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(getOneStatistics.Token) || Program.LoggedInUsers[getOneStatistics.Token].Id != getOneStatistics.Id)
                {
                    return Unauthorized(new ErrorDTO() { Id = 116, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == getOneStatistics.Id);

                if (userStatistic is null)
                {
                    return NotFound(new ErrorDTO() { Id = 117, Message = "Az elem nem található" });
                }

                Dictionary<string, int[]> successRates = new Dictionary<string, int[]>();


                if (getOneStatistics.SubjectIds.Length != 0)
                {
                    await GetSubjects(getOneStatistics, userStatistic, successRates);
                    

                }

                return Ok(successRates);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 118, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 119, Message = "Hiba történt az adatok lekérdezése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 120, Message = "Hiba történt az adatok lekérdezése közben" });
            }

        }

        //szures add: szint, (feladatok megjelenítése statisztikával egyszerre max 50 szürés: tanátrgy, szint, téma)
        [HttpPost("get-one-statistics-filter")]
        public async Task<ActionResult<UserStatistic>> GetUserStatisticFilter([FromBody] GetOneFilterStatisticsDTO filterStatisticsDTO)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(filterStatisticsDTO.Token) || Program.LoggedInUsers[filterStatisticsDTO.Token].Id != filterStatisticsDTO.Id)
                {
                    return Unauthorized(new ErrorDTO() { Id = 134, Message = "Hozzáférés megtagadva" });//hibaüzenet átírás szám
                }

                UserStatistic userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == filterStatisticsDTO.Id);

                if (userStatistic is null)
                {
                    return NotFound(new ErrorDTO() { Id = 135, Message = "Az elem nem található" });
                }

                Dictionary<Subject, int[]> successRates = new Dictionary<Subject, int[]>();

                if (true)
                {
                    //await GetSubjects(getOneStatistics, userStatistic, successRates);
                }

                return Ok(successRates);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 136, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 137, Message = "Hiba történt az adatok lekérdezése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 138, Message = "Hiba történt az adatok lekérdezése közben" });
            }

        }

        [HttpPut("put-statisztika")]
        public async Task<IActionResult> PutUserStatistic([FromBody] PutStatisticsDTO putStatistics)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(putStatistics.Token) && Program.LoggedInUsers[putStatistics.Token].Id != putStatistics.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 121, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic? userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == putStatistics.UserId);

                if (userStatistic is null)
                {
                    return BadRequest(new ErrorDTO() { Id = 122, Message = "Ilyen nevű felhasználó nem létezik" });
                }

                foreach (int id in putStatistics.TaskIds.Keys)
                {
                    if (putStatistics.TaskIds[id])
                    {
                        await PutStatistics(putStatistics, userStatistic, id, true);
                        continue;
                    }

                    await PutStatistics(putStatistics, userStatistic, id, false);
                }

                _context.Entry(userStatistic).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 123, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 124, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 125, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Adatok sikeresen elmentve");
        }

        [HttpPost("post-user-stat")]
        public async Task<ActionResult<UserStatistic>> PostUserStatistic([FromBody] int userid)
        {
            try
            {
                UserStatistic userStatistic = new UserStatistic()
                {
                    UserId = userid
                };

                _context.UserStatistics.Add(userStatistic);
                await _context.SaveChangesAsync();
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 130, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 131, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 132, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok();
        }
        
        private async Task GetSubjects(GetOneStatisticsDTO getOneStatistics, UserStatistic userStatistic, Dictionary<string, int[]> successRates)
        {
            for (int i = 0; i < getOneStatistics.SubjectIds.Length; i++)
            {
                int seged = 0;
                switch (getOneStatistics.SubjectIds[i])
                {
                    case 1:
                        seged = userStatistic.MathSuccessfulTasks.Split(",").Count();
                        int mathSuccesfullTask = seged < 0 ? 0 : seged;
                        seged = userStatistic.MathUnsuccessfulTasks.Split(",").Count();
                        int mathUnSuccesfullTask = seged < 0 ? 0 : seged;
                        successRates.Add("Matek", new int[] { (int)Math.Round((double)mathSuccesfullTask * 100 / (mathSuccesfullTask + mathUnSuccesfullTask)), mathSuccesfullTask + mathUnSuccesfullTask });
                        break;


                    case 2:
                        seged = userStatistic.HistorySuccessfulTasks.Split(",").Count();
                        int historySuccessfulTasks = seged < 0 ? 0 : seged; 
                        seged = userStatistic.HistoryUnsuccessfulTasks.Split(",").Count();
                        int historyUnsuccessfulTasks = seged < 0 ? 0 : seged;
                        successRates.Add("Történelem", new int[] { (int)Math.Round((double)historySuccessfulTasks * 100 / (historySuccessfulTasks + historyUnsuccessfulTasks)), historySuccessfulTasks + historyUnsuccessfulTasks });
                        break;


                    case 3:
                        seged = userStatistic.HungarianSuccessfulTasks.Split(",").Count();
                        int hungarianSuccessfulTasks = seged < 0 ? 0 : seged;
                        seged = userStatistic.HungarianUnsuccessfulTasks.Split(",").Count();
                        int hungarianUnsuccessfulTasks = seged < 0 ? 0 : seged;
                        successRates.Add("Magyar", new int[] { (int)Math.Round((double)hungarianSuccessfulTasks * 100 / (hungarianSuccessfulTasks + hungarianUnsuccessfulTasks)), hungarianSuccessfulTasks + hungarianUnsuccessfulTasks });
                        break;
                }

            }
        }

        private async Task PutStatistics(PutStatisticsDTO putStatistics, UserStatistic userStatistic, int taskId, bool success)
        {
            switch (putStatistics.SubjectId)
            {
                case 1:
                    if (success)
                    {
                        userStatistic.MathSuccessfulTasks += $"{taskId},";
                        break;
                    }

                    userStatistic.MathUnsuccessfulTasks += $"{taskId},";
                    break;


                case 2:
                    if (success)
                    {
                        userStatistic.HistorySuccessfulTasks += $"{taskId},";
                        break;
                    }

                    userStatistic.HistoryUnsuccessfulTasks += $"{taskId},";
                    break;


                case 3:
                    if (success)
                    {
                        userStatistic.HungarianSuccessfulTasks += $"{taskId},";
                        break;
                    }

                    userStatistic.HungarianUnsuccessfulTasks += $"{taskId},";
                    break;
            }
        }
    }
}
