using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.DTOs;

namespace ErettsegizzunkApi.Controllers
{
    [Route("api/[controller]")]
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
            return await _context.UserStatistics.ToListAsync();
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
            return NoContent();
        }

        // POST: api/UserStatistics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post-user-statistics")]
        public async Task<ActionResult<UserStatistic>> PostUserStatistic(PostStatisticsDTO postStatistics)
        {

            foreach (int taskId in postStatistics.TaskIds.Keys)
            {
                UserStatistic userStatistic = new UserStatistic()
                {
                    UserId = postStatistics.UserId,
                    TaskId = taskId,
                    IsSuccessful = postStatistics.TaskIds[taskId]
                };

                _context.UserStatistics.Add(userStatistic);
            }
            
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
    }
}
