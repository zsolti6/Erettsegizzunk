using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Type = ErettsegizzunkApi.Models.Type;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;

        public TypesController(ErettsegizzunkContext context)//============>>>>>>>>>>>>>>>>>>> Hibakezel!!!!!!!!!!!
        {
            _context = context;
        }

        //Típusok lekérdezése
        [HttpGet("get-tipusok")]
        public async Task<ActionResult<IEnumerable<List<Type>>>> GetTypes()
        {
            try
            {
                return Ok(await _context.Types.ToListAsync());
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 162, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 163, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }
    }
}
