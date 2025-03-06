using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.DTOs;
using NuGet.Packaging;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Hosting;

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

        [HttpPost("get-match-history")]
        public async Task<IActionResult> GetMatchHistory([FromBody] FilteredDeatiledDTO filteredDeatiled)
        {

            try
            {
                if (!Program.LoggedInUsers.ContainsKey(filteredDeatiled.Token) || Program.LoggedInUsers[filteredDeatiled.Token].Id != filteredDeatiled.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 116, Message = "Hozzáférés megtagadva" });
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
                return BadRequest(new ErrorDTO() { Id = 118, Message = "Hiba történt az adatok lekérdezése közben" });
            }


        }


        [HttpPost("get-statitstics-detailed")]
        public async Task<IActionResult> GetStatisticsDeatiled([FromBody] FilteredDeatiledDTO filteredDeatiled)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(filteredDeatiled.Token) || Program.LoggedInUsers[filteredDeatiled.Token].Id != filteredDeatiled.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 119, Message = "Hozzáférés megtagadva" });
                }

                List<FilteredTaskDTO> filteredTasks = new List<FilteredTaskDTO>();

                filteredTasks = _context.UserStatistics
                    .Include(x => x.Task)
                    .Include(x => x.Task.Subject)
                    .Include(x => x.Task.Level)
                    .Where(x => x.UserId == filteredDeatiled.UserId)
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
                    .ToList();

                return Ok(filteredTasks);
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 120, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 121, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }


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
                return BadRequest(new ErrorDTO() { Id = 124, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }


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
                return NotFound(new ErrorDTO() { Id = 128, Message = "Hiba történt az adatok mentése közben" });
            }

            return Ok();
        }

        [HttpPost("get-filling-byDate")]
        public async Task<ActionResult<UserStatistic>> GetFillingByDate([FromBody] GetFillingCountDTO fillingByDateCount)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(fillingByDateCount.Token) || Program.LoggedInUsers[fillingByDateCount.Token].Id != fillingByDateCount.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 18, Message = "Hozzáférés megtagadva" });
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
                return StatusCode(500, new ErrorDTO() { Id = 2, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 3, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        [HttpDelete("statisztika-reset")]
        public async Task<IActionResult> DeleteUserStatistic([FromBody] StatisticsResetDTO statisticsReset)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(statisticsReset.Token) || Program.LoggedInUsers[statisticsReset.Token].Id != statisticsReset.UserId)
                {
                    return Unauthorized(new ErrorDTO() { Id = 18, Message = "Hozzáférés megtagadva" });
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
                return StatusCode(500, new ErrorDTO() { Id = 20, Message = "Kapcsolati hiba" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 21, Message = "Hiba történt az adatok mentése közben" });
            }
            catch (Exception)
            {
                return NotFound(new ErrorDTO() { Id = 22, Message = "Hiba történt az adatok mentése közben" });
            }


            return NoContent();
        }
    }
}
