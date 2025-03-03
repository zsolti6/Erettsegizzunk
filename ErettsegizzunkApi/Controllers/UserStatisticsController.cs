using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Task = ErettsegizzunkApi.Models.Task;

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
                    //return Unauthorized(new ErrorDTO() { Id = 116, Message = "Hozzáférés megtagadva" });
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
        public async Task<ActionResult<UserStatistic>> GetUserStatisticFilter([FromBody] GetOneFilterStatisticsDTO getOneFilter)//NINCS DOKUMENTÁLVA
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(getOneFilter.Token) || Program.LoggedInUsers[getOneFilter.Token].Id != getOneFilter.Id)
                {
                    //return Unauthorized(new ErrorDTO() { Id = 134, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == getOneFilter.Id);

                if (userStatistic is null)
                {
                    return NotFound(new ErrorDTO() { Id = 135, Message = "Az elem nem található" });
                }

                List<FilteredTaksDTO> filteredTaks = new List<FilteredTaksDTO>();

                if (true)
                {
                    filteredTaks = await GetSubjectSuccesfullUnsuccesfullCount(getOneFilter,userStatistic);
                }

                return Ok(filteredTaks);
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

        [HttpPut("put-statisztika")] //nem a végére hanem az elejésre kell a vessző
        public async Task<IActionResult> PutUserStatistic([FromBody] PutStatisticsDTO putStatistics)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(putStatistics.Token) && Program.LoggedInUsers[putStatistics.Token].Id != putStatistics.UserId)
                {
                    //return Unauthorized(new ErrorDTO() { Id = 121, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic? userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == putStatistics.UserId);

                if (userStatistic is null)
                {
                    return BadRequest(new ErrorDTO() { Id = 122, Message = "Ilyen nevű felhasználó nem létezik" });
                }

                //var asd = putStatistics.TaskIds.Values.ToList()[0];

                for (int i = 0; i < putStatistics.TaskIds.Keys.Count; i++)
                {
                    if (putStatistics.TaskIds.Values.ToList()[i])
                    {
                        await PutStatistics(putStatistics, userStatistic, i + 1, true, i);
                        continue;
                    }

                    await PutStatistics(putStatistics, userStatistic, i + 1, false, i);
                }

                userStatistic.StatisticsDates += DateTime.Now.ToString("yyyy-MM-dd") + ";";

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
            catch (Exception ex)
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

        private async Task<List<FilteredTaksDTO>> GetSubjectSuccesfullUnsuccesfullCount(GetOneFilterStatisticsDTO getOneFilter, UserStatistic userStatistic)
        {
            List<FilteredTaksDTO> feladatok = new List<FilteredTaksDTO>();
            HashSet<int> ids = new HashSet<int>();
            List<int> joDb = new List<int>();
            List<int> rosszDb = new List<int>();
            string[] tasks;
            string[] joSplit;
            string[] rosszSplit;
            string seged = string.Empty;

            switch (getOneFilter.SubjectId)
            {
                case 1:
                    seged = userStatistic.MathSuccessfulTasks + userStatistic.MathUnsuccessfulTasks;
                    tasks = seged.Remove(seged.Length - 1).Split(new char[] { ',', ';' });

                    joSplit = userStatistic.MathSuccessfulTasks.Split(new char[] { ',', ';' });
                    rosszSplit = userStatistic.MathSuccessfulTasks.Split(new char[] { ',', ';' });
                    FeladatIdLevalogatas(ids, joDb, rosszDb, tasks, joSplit, rosszSplit);
                    break;

                case 2:
                    seged = userStatistic.HistorySuccessfulTasks + userStatistic.HistoryUnsuccessfulTasks;
                    tasks = seged.Remove(seged.Length - 1).Split(new char[] { ',', ';' });

                    joSplit = userStatistic.HistorySuccessfulTasks.Split(new char[] { ',', ';' });
                    rosszSplit = userStatistic.HistoryUnsuccessfulTasks.Split(new char[] { ',', ';' });
                    FeladatIdLevalogatas(ids, joDb, rosszDb, tasks, joSplit, rosszSplit);
                    break;

                case 3:
                    seged = userStatistic.HungarianSuccessfulTasks + userStatistic.HungarianUnsuccessfulTasks;
                    tasks = seged.Remove(seged.Length - 1).Split(new char[] { ',', ';' });

                    joSplit = userStatistic.HungarianSuccessfulTasks.Split(new char[] { ',', ';' });
                    rosszSplit = userStatistic.HungarianUnsuccessfulTasks.Split(new char[] { ',', ';' });
                    FeladatIdLevalogatas(ids, joDb, rosszDb, tasks, joSplit, rosszSplit);
                    break;

            }

            List<Task> taskList = await _context.Tasks
                .Where(x => ids.Contains(x.Id))//lapozást ide berakni
                .Take(50)
                .ToListAsync();

            for (int i = 0; i < taskList.Count; i++)
            {
                FilteredTaksDTO filteredTaks = new FilteredTaksDTO()
                {
                    Task = taskList[i],
                    JoRossz = new int[] { joDb[i], rosszDb[i] },
                };
                feladatok.Add(filteredTaks);
            }
            return feladatok;
        }

        private static void FeladatIdLevalogatas(HashSet<int> ids, List<int> joDb, List<int> rosszDb, string[] tasks, string[] joSplit, string[] rosszSplit)
        {
            foreach (string item in tasks)
            {
                joDb.Add(joSplit.Count(x => x == item));
                rosszDb.Add(rosszSplit.Count(x => x == item));
                ids.Add(int.Parse(item));
            }
        }

        private async System.Threading.Tasks.Task GetSubjects(GetOneStatisticsDTO getOneStatistics, UserStatistic userStatistic, Dictionary<string, int[]> successRates)
        {
            for (int i = 0; i < getOneStatistics.SubjectIds.Length; i++)
            {
                int seged = 0;
                switch (getOneStatistics.SubjectIds[i])
                {
                    case 1:
                        seged = userStatistic.MathSuccessfulTasks.Remove(userStatistic.MathSuccessfulTasks.Length - 1).Split(new char[] {',',';'}).Count();
                        int mathSuccesfullTask = seged < 0 ? 0 : seged;
                        seged = userStatistic.MathUnsuccessfulTasks.Remove(userStatistic.MathUnsuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }).Count();
                        int mathUnSuccesfullTask = seged < 0 ? 0 : seged;
                        successRates.Add("Matek", new int[] { (int)Math.Round((double)mathSuccesfullTask * 100 / (mathSuccesfullTask + mathUnSuccesfullTask)), mathSuccesfullTask + mathUnSuccesfullTask });
                        break;


                    case 2:
                        seged = userStatistic.HistorySuccessfulTasks.Remove(userStatistic.HistorySuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }).Count();
                        int historySuccessfulTasks = seged < 0 ? 0 : seged; 
                        seged = userStatistic.HistoryUnsuccessfulTasks.Remove(userStatistic.HistoryUnsuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }).Count();
                        int historyUnsuccessfulTasks = seged < 0 ? 0 : seged;
                        successRates.Add("Történelem", new int[] { (int)Math.Round((double)historySuccessfulTasks * 100 / (historySuccessfulTasks + historyUnsuccessfulTasks)), historySuccessfulTasks + historyUnsuccessfulTasks });
                        break;


                    case 3:
                        seged = userStatistic.HungarianSuccessfulTasks.Remove(userStatistic.HungarianSuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }).Count();
                        int hungarianSuccessfulTasks = seged < 0 ? 0 : seged;
                        seged = userStatistic.HungarianUnsuccessfulTasks.Remove(userStatistic.HungarianUnsuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }).Count();
                        int hungarianUnsuccessfulTasks = seged < 0 ? 0 : seged;
                        successRates.Add("Magyar", new int[] { (int)Math.Round((double)hungarianSuccessfulTasks * 100 / (hungarianSuccessfulTasks + hungarianUnsuccessfulTasks)), hungarianSuccessfulTasks + hungarianUnsuccessfulTasks });
                        break;
                }

            }
        }

        //lehet h bugos tesztelni kell
        private async System.Threading.Tasks.Task PutStatistics(PutStatisticsDTO putStatistics, UserStatistic userStatistic, int taskId, bool success, int hol)
        {
            switch (putStatistics.SubjectId)
            {
                case 1:
                    if (success)
                    {
                        userStatistic.MathSuccessfulTasks += $"{taskId},";
                    }
                    else
                    {
                        userStatistic.MathUnsuccessfulTasks += $"{taskId},";
                    }

                    if (hol == 14)
                    {
                        userStatistic.MathSuccessfulTasks = userStatistic.MathSuccessfulTasks.Substring(0, userStatistic.MathSuccessfulTasks.Length - 1) + ';';
                        userStatistic.MathUnsuccessfulTasks = userStatistic.MathUnsuccessfulTasks.Substring(0, userStatistic.MathUnsuccessfulTasks.Length - 1) + ';';
                    }
                    break;


                case 2:
                    if (success)
                    {
                        userStatistic.HistorySuccessfulTasks += $"{taskId},";
                    }
                    else
                    {
                        userStatistic.HistoryUnsuccessfulTasks += $"{taskId},";
                    }

                    if (hol == 14)
                    {
                        userStatistic.HistorySuccessfulTasks = userStatistic.HistorySuccessfulTasks.Substring(0, userStatistic.HistorySuccessfulTasks.Length - 1) + ';';
                        userStatistic.HistoryUnsuccessfulTasks = userStatistic.HistoryUnsuccessfulTasks.Substring(0, userStatistic.HistoryUnsuccessfulTasks.Length - 1) + ';';
                    }

                    break;


                case 3:
                    if (success)
                    {
                        userStatistic.HungarianSuccessfulTasks += $"{taskId},";
                    }
                    else
                    {
                        userStatistic.HungarianUnsuccessfulTasks += $"{taskId},";
                    }

                    if (hol == 14)
                    {
                        userStatistic.HungarianSuccessfulTasks = userStatistic.HungarianSuccessfulTasks.Substring(0, userStatistic.HungarianSuccessfulTasks.Length - 1) + ';';
                        userStatistic.HungarianUnsuccessfulTasks = userStatistic.HungarianUnsuccessfulTasks.Substring(0, userStatistic.HungarianUnsuccessfulTasks.Length - 1) + ';';
                    }

                    break;
            }
        }
    }
}
