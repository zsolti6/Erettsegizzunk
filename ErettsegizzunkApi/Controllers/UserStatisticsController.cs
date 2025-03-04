using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Linq;
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
            catch (Exception ex)
            {
                return NotFound(new ErrorDTO() { Id = 120, Message = "Hiba történt az adatok lekérdezése közben" });
            }

        }

        //szures add: szint, (feladatok megjelenítése statisztikával egyszerre max 50 szürés: tanátrgy, szint, téma) ==> kész: tantárgy
        [HttpPost("get-one-statistics-filter")]
        public async Task<ActionResult<UserStatistic>> GetUserStatisticFilter([FromBody] GetOneFilterStatisticsDTO getOneFilter)
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

                if (true)//??
                {
                    filteredTaks = await GetSubjectSuccesfullUnsuccesfullCount(getOneFilter, userStatistic);
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
            catch (Exception ex)
            {
                return NotFound(new ErrorDTO() { Id = 138, Message = "Hiba történt az adatok lekérdezése közben" });
            }

        }

        [HttpPost("get-filling-byDate")]
        public async Task<IActionResult> GetFillingByDate(GetFillingByDateDTO getFillingByDate) //dokumentálni
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(getFillingByDate.Token) || Program.LoggedInUsers[getFillingByDate.Token].Id != getFillingByDate.UserId)
                {
                    //return Unauthorized(new ErrorDTO() { Id = 139, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == getFillingByDate.UserId);

                if (userStatistic is null)
                {
                    return NotFound(new ErrorDTO() { Id = 140, Message = "Az elem nem található" });
                }

                Dictionary<string, int> statisztika = new Dictionary<string, int>();

                statisztika = _context.UserStatistics
                            .AsEnumerable()
                            .Where(x => x.UserId == getFillingByDate.UserId)
                            .SelectMany(x => x.StatisticsDates.Split(';', StringSplitOptions.RemoveEmptyEntries))
                            .GroupBy(date => date)
                            .ToDictionary(x => x.Key, x => x.Count());//atirni

                return Ok(statisztika);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 141, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 142, Message = "Hiba történt az adatok lekérdezése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 143, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        [HttpPut("put-statisztika")] //nem a végére hanem az elejésre kell a vessző
        public async Task<IActionResult> PutUserStatistic([FromBody] PutStatisticsDTO putStatistics)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(putStatistics.Token) || Program.LoggedInUsers[putStatistics.Token].Id != putStatistics.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 121, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic? userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == putStatistics.UserId);

                if (userStatistic is null)
                {
                    return BadRequest(new ErrorDTO() { Id = 122, Message = "Ilyen nevű felhasználó nem létezik" });
                }

                int i = 0;

                foreach (int item in putStatistics.TaskIds.Keys)
                {
                    if (putStatistics.TaskIds.Values.ToList()[i])
                    {
                        await PutStatistics(putStatistics, userStatistic, item, true, i);
                        i++;
                        continue;
                    }

                    await PutStatistics(putStatistics, userStatistic, item, false, i);

                    i++;
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
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 125, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok("Adatok sikeresen elmentve");
        }

        [HttpPut("reset-user-stat")]
        public async Task<IActionResult> ResetStats(LoggedUserDTO loggedUser)//DOKUMENTÁLNI
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(loggedUser.Token) || Program.LoggedInUsers[loggedUser.Token].Id != loggedUser.Id)
                {
                    return Unauthorized(new ErrorDTO() { Id = 121, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic? userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == loggedUser.Id);

                if (userStatistic is null)
                {
                    return BadRequest(new ErrorDTO() { Id = 122, Message = "Ilyen nevű felhasználó nem létezik" });
                }

                userStatistic.StatisticsDates = string.Empty;
                userStatistic.MathSuccessfulTasks = string.Empty;
                userStatistic.MathUnsuccessfulTasks = string.Empty;
                userStatistic.HistorySuccessfulTasks = string.Empty;
                userStatistic.HistoryUnsuccessfulTasks = string.Empty;
                userStatistic.HungarianSuccessfulTasks = string.Empty;
                userStatistic.HungarianUnsuccessfulTasks = string.Empty;

                _context.Entry(userStatistic).State = EntityState.Modified;
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

        #region Local functions

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

            foreach (int item in getOneFilter.SubjectIds)
            {
                switch (item)
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
                .Where(x => ids.Contains(x.Id))//lapozás
                .Take(50)
                .ToListAsync();

                List<UserStatistic> statisticsList = await _context.UserStatistics.Where(x => x.UserId == userStatistic.UserId).ToListAsync();
                List<string> dates = new List<string>();
                List<string> taskIdList = new List<string>();

                foreach (UserStatistic usersStats in statisticsList)
                {
                    dates = usersStats.StatisticsDates.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
                    taskIdList = (usersStats.MathSuccessfulTasks + usersStats.MathUnsuccessfulTasks + usersStats.HistorySuccessfulTasks + usersStats.HistoryUnsuccessfulTasks + usersStats.HungarianSuccessfulTasks + usersStats.HungarianUnsuccessfulTasks).Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
                }


                for (int i = 0; i < taskList.Count; i++)//################# ÁTÍRNI BUGOS MINDIG AZ UTOLSÓ SUBJECT IDSAT JELENÍTI CSAK MEG!!
                {

                    string searchId = taskList[i].Id.ToString();
                    int lastIndex = -1;

                    // Search for the last occurrence
                    for (int row = taskIdList.Count - 1; row >= 0; row--)
                    {
                        var idsInRow = taskIdList[row].Split(',', StringSplitOptions.RemoveEmptyEntries);
                        if (idsInRow.Contains(searchId))
                        {
                            lastIndex = row;
                            break; // Stop once we find the last occurrence
                        }
                    }

                    // Get the corresponding date or default to "N/A"
                    string asd = lastIndex != -1 ? dates[lastIndex] : "N/A";


                    FilteredTaksDTO filteredTaks = new FilteredTaksDTO()
                    {
                        Task = taskList[i],
                        JoRossz = new int[] { joDb[i], rosszDb[i] },
                        UtolsoKitoltesDatum = asd,
                        UtolsoSikeres = true
                    };
                    feladatok.Add(filteredTaks);
                }

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

        private async System.Threading.Tasks.Task GetSubjects(GetOneStatisticsDTO getOneStatistics, UserStatistic userStatistic, Dictionary<string, int[]> successRates)//stringsplitoptions
        {
            for (int i = 0; i < getOneStatistics.SubjectIds.Length; i++)
            {
                int seged = 0;
                int szazalek = 0;
                switch (getOneStatistics.SubjectIds[i])
                {
                    case 1:
                        if (userStatistic.MathSuccessfulTasks.Length > 0)
                        {
                            seged = userStatistic.MathSuccessfulTasks.Remove(userStatistic.MathSuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        }
                        int mathSuccesfullTask = seged < 0 ? 0 : seged;

                        seged = 0;
                        if (userStatistic.MathUnsuccessfulTasks.Length > 0)
                        {
                            seged = userStatistic.MathUnsuccessfulTasks.Remove(userStatistic.MathUnsuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        }
                        int mathUnSuccesfullTask = seged < 0 ? 0 : seged;
                        szazalek = (int)Math.Round((double)mathSuccesfullTask * 100 / (mathSuccesfullTask + mathUnSuccesfullTask));
                        successRates.Add("Matek", new int[] { szazalek == int.MinValue ? 0 : szazalek, mathSuccesfullTask + mathUnSuccesfullTask });
                        break;


                    case 2:
                        if (userStatistic.HistorySuccessfulTasks.Length > 0)
                        {
                            seged = userStatistic.HistorySuccessfulTasks.Remove(userStatistic.HistorySuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        }
                        int historySuccessfulTasks = seged < 0 ? 0 : seged;

                        seged = 0;
                        if (userStatistic.HistoryUnsuccessfulTasks.Length > 0)
                        {
                            seged = userStatistic.HistoryUnsuccessfulTasks.Remove(userStatistic.HistoryUnsuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        }

                        int historyUnsuccessfulTasks = seged < 0 ? 0 : seged;
                        szazalek = (int)Math.Round((double)historySuccessfulTasks * 100 / (historySuccessfulTasks + historyUnsuccessfulTasks));
                        successRates.Add("Történelem", new int[] { szazalek == int.MinValue ? 0 : szazalek, historySuccessfulTasks + historyUnsuccessfulTasks });
                        break;


                    case 3:
                        if (userStatistic.HungarianSuccessfulTasks.Length > 0)
                        {
                            seged = userStatistic.HungarianSuccessfulTasks.Remove(userStatistic.HungarianSuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        }
                        int hungarianSuccessfulTasks = seged < 0 ? 0 : seged;

                        seged = 0;
                        if (userStatistic.HungarianUnsuccessfulTasks.Length > 0)
                        {
                            seged = userStatistic.HungarianUnsuccessfulTasks.Remove(userStatistic.HungarianUnsuccessfulTasks.Length - 1).Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Count();
                        }

                        int hungarianUnsuccessfulTasks = seged < 0 ? 0 : seged;
                        szazalek = (int)Math.Round((double)hungarianSuccessfulTasks * 100 / (hungarianSuccessfulTasks + hungarianUnsuccessfulTasks));
                        successRates.Add("Magyar", new int[] { szazalek == int.MinValue ? 0 : szazalek, hungarianSuccessfulTasks + hungarianUnsuccessfulTasks });
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

                    if (hol == 14 && userStatistic.MathSuccessfulTasks.Length > 0)
                    {
                        userStatistic.MathSuccessfulTasks = userStatistic.MathSuccessfulTasks.Substring(0, userStatistic.MathSuccessfulTasks.Length - 1) + ';';
                    }

                    if (hol == 14 && userStatistic.MathUnsuccessfulTasks.Length > 0)
                    {
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

                    if (hol == 14 && userStatistic.HistorySuccessfulTasks.Length > 0)
                    {
                        userStatistic.HistorySuccessfulTasks = userStatistic.HistorySuccessfulTasks.Substring(0, userStatistic.HistorySuccessfulTasks.Length - 1) + ';';
                    }

                    if (hol == 14 && userStatistic.HistoryUnsuccessfulTasks.Length > 0)
                    {
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

                    if (hol == 14 && userStatistic.HungarianSuccessfulTasks.Length > 0)
                    {
                        userStatistic.HungarianSuccessfulTasks = userStatistic.HungarianSuccessfulTasks.Substring(0, userStatistic.HungarianSuccessfulTasks.Length - 1) + ';';
                    }

                    if (hol == 14 && userStatistic.HungarianUnsuccessfulTasks.Length > 0)
                    {
                        userStatistic.HungarianUnsuccessfulTasks = userStatistic.HungarianUnsuccessfulTasks.Substring(0, userStatistic.HungarianUnsuccessfulTasks.Length - 1) + ';';
                    }

                    break;
            }
        }
        #endregion
    }
}
