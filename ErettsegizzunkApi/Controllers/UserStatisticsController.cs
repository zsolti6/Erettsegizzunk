using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErettsegizzunkApi.Models;

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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStatisticExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserStatistics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserStatistic>> PostUserStatistic(UserStatistic userStatistic)
        {
            _context.UserStatistics.Add(userStatistic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserStatistic", new { id = userStatistic.Id }, userStatistic);
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

        private bool UserStatisticExists(int id)
        {
            return _context.UserStatistics.Any(e => e.Id == id);
        }
    }
}
