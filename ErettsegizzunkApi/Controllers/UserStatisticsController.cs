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
        public async Task<ActionResult<IEnumerable<UserStatistic>>> GetUserStatistics()
        {
            return await _context.UserStatistics.ToListAsync();
        }

        [HttpPost("get-one-statistics")]
        public async Task<ActionResult<UserStatistic>> GetUserStatistic([FromBody] GetOneStatisticsDTO getOneStatistics)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(getOneStatistics.Token) && Program.LoggedInUsers[getOneStatistics.Token].Id != getOneStatistics.Id)
                {
                    return BadRequest(new ErrorDTO() { Id = 116, Message = "Hozzáférés megtagadva" });
                }

                UserStatistic userStatistic = await _context.UserStatistics.FirstOrDefaultAsync(x => x.UserId == getOneStatistics.Id);

                if (userStatistic == null)
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

        // PUT: api/UserStatistics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserStatistic(int id, UserStatistic userStatistic)
        {
            if (id != userStatistic.Id)
            {
                return BadRequest();
            }

            _context.Entry(userStatistic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }

        [HttpPost("post-user-stat")]
        public async Task<ActionResult<UserStatistic>> PostUserStatistic([FromBody] int userid)
        {
            UserStatistic userStatistic = new UserStatistic()
            {
                UserId = userid
            };

            _context.UserStatistics.Add(userStatistic);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/UserStatistics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserStatistic(int id)
        {
            var userStatistic = await _context.UserStatistics.FindAsync(id);
            if (userStatistic == null)
            {
                return NotFound();
            }

            _context.UserStatistics.Remove(userStatistic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task GetSubjects(GetOneStatisticsDTO getOneStatistics, UserStatistic userStatistic, Dictionary<string, int> successRates)
        {
            for (int i = 0; i < getOneStatistics.SubjectIds.Length; i++)
            {

                switch (getOneStatistics.SubjectIds[i])
                {
                    case 1:
                        string[] mathSuccesfullTask = userStatistic.MathSuccessfulTasks.Split(";");
                        string[] mathUnsuccessfulTask = userStatistic.MathUnsuccessfulTasks.Split(";");
                        await GetPercentage(mathSuccesfullTask, mathUnsuccessfulTask, successRates, "Matek");
                        break;


                    case 2:
                        string[] historySuccesfullTask = userStatistic.HistorySuccessfulTasks.Split(";");
                        string[] historyUnsuccessfulTask = userStatistic.HistoryUnsuccessfulTasks.Split(";");
                        await GetPercentage(historySuccesfullTask, historyUnsuccessfulTask, successRates, "Történelem");
                        break;


                    case 3:

                        string[] hungarianSuccesfullTask = userStatistic.HungarianSuccessfulTasks.Split(";");
                        string[] hingarianUnsuccessfulTask = userStatistic.HungarianUnsuccessfulTasks.Split(";");
                        await GetPercentage(hungarianSuccesfullTask, hingarianUnsuccessfulTask, successRates, "Magyar");
                        break;
                }

            }
        }

        private async Task GetPercentage(string[] succesFull, string[] unSuccessFull, Dictionary<string, int> successRates, string nev)
        {
            int sikerDarab = 0;
            int sikertelenDarab = 0;

            foreach (string item in succesFull)
            {
                sikerDarab += item.Split(',').Count();
            }

            foreach (string item in unSuccessFull)
            {
                sikertelenDarab += item.Split(',').Count();
            }

            successRates.Add(nev, (int)Math.Round((double)sikerDarab * 100 / (sikerDarab + sikertelenDarab)));
        }
    }
}
