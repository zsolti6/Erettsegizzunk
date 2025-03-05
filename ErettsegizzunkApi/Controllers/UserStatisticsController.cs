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

        // GET: api/UserStatistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserStatistic>>> GetUserStatistics()
        {
            return Ok(_context.UserStatistics.Count());
        }

        // GET: api/UserStatistics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserStatistic>> GetUserStatistic(int id)
        {
            var userStatistic = await _context.UserStatistics.FindAsync(id);

            if (userStatistic == null)
            {
                return NotFound();
            }

            return userStatistic;
        }


        [HttpPost("get-taskFilloutCount")]
        public async Task<IActionResult> GetTaskFilloutCount([FromBody] GetFillingCountDTO getFillingCount)
        {
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == getFillingCount.UserId);

                if (user is null)
                {
                    return NotFound();
                }

                Dictionary<string, int> taskFilloutCount = new Dictionary<string, int>();
                taskFilloutCount = await _context.UserStatistics
                    .Include(x => x.Task.Subject)
                    .Where(x => x.UserId == getFillingCount.UserId)
                    .GroupBy(x => x.Task.Subject!.Name)
                    .ToDictionaryAsync(g => g.Key!, g => g.Select(x => x.TaskId).Count());

                return Ok(taskFilloutCount);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost("post-user-statistics")]
        public async Task<ActionResult<UserStatistic>> PostUserStatistic([FromBody] PostStatisticsDTO postStatistics)
        {
            try
            {
                foreach (int taskId in postStatistics.TaskIds.Keys)
                {
                    UserStatistic userStatistic = new UserStatistic()
                    {
                        UserId = postStatistics.UserId,
                        TaskId = taskId,
                        IsSuccessful = postStatistics.TaskIds[taskId],
                        FilloutDate = DateTime.Now
                    };

                    _context.UserStatistics.Add(userStatistic);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            
            return Ok();
        }

        [HttpPost("get-filling-byDate")]
        public async Task<ActionResult<UserStatistic>> GetFillingByDate([FromBody] GetFillingCountDTO fillingByDateCount)
        {
            try
            {
                Dictionary<string, int> taskFilloutCount = new Dictionary<string, int>();
                taskFilloutCount = await _context.UserStatistics
                    .Where(x => x.UserId == fillingByDateCount.UserId)
                    .GroupBy(x => x.FilloutDate.ToString())
                    .ToDictionaryAsync(g => g.Key!, g => g.Count());

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

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
    }
}
