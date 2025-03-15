using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return Ok(await _context.Types.ToListAsync());
        }
    }
}
