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
                if (!Program.LoggedInUsers.ContainsKey(getOneStatistics.Token) && Program.LoggedInUsers[getOneStatistics.Token].Id != getOneStatistics.Id)
                {
                    return Unauthorized(new ErrorDTO() { Id = 116, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == getOneStatistics.Id);

                if (userStatistic is null)
                {
                    return NotFound(new ErrorDTO() { Id = 117, Message = "Az elem nem található" });
                }

                Dictionary<string, int> successRates = new Dictionary<string, int>();

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

        private async Task GetSubjects(GetOneStatisticsDTO getOneStatistics, UserStatistic userStatistic, Dictionary<string, int> successRates)
        {
            for (int i = 0; i < getOneStatistics.SubjectIds.Length; i++)
            {

                switch (getOneStatistics.SubjectIds[i])
                {
                    case 1:
                        int mathSuccesfullTask = userStatistic.MathSuccessfulTasks.Replace(",", "").Count();
                        int mathUnSuccesfullTask = userStatistic.MathUnsuccessfulTasks.Replace(",", "").Count();
                        successRates.Add("Matek", (int)Math.Round((double)mathSuccesfullTask * 100 / (mathSuccesfullTask + mathUnSuccesfullTask)));
                        break;


                    case 2:
                        int historySuccessfulTasks = userStatistic.HistorySuccessfulTasks.Replace(",", "").Count();
                        int historyUnsuccessfulTasks = userStatistic.HistoryUnsuccessfulTasks.Replace(",", "").Count();
                        successRates.Add("Történelem", (int)Math.Round((double)historySuccessfulTasks * 100 / (historySuccessfulTasks + historyUnsuccessfulTasks)));
                        break;


                    case 3:
                        int hungarianSuccessfulTasks = userStatistic.HungarianSuccessfulTasks.Replace(",", "").Count();
                        int hungarianUnsuccessfulTasks = userStatistic.HungarianUnsuccessfulTasks.Replace(",", "").Count();
                        successRates.Add("Magyar", (int)Math.Round((double)hungarianSuccessfulTasks * 100 / (hungarianSuccessfulTasks + hungarianUnsuccessfulTasks)));
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
