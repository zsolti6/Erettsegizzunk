using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        //User token frissítése
        [HttpPost("refresh-token-name")]
        public IActionResult TokenRefreshName([FromBody] TokenRefreshDTO tokenRefresh)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(tokenRefresh.Token) || Program.LoggedInUsers[tokenRefresh.Token].LoginName != tokenRefresh.OldName)
                {
                    return Unauthorized(new ErrorDTO() { Id = 89, Message = "Hozzáférés megtagadva" });//-----> hibakezelés
                }

                Program.LoggedInUsers[tokenRefresh.Token].LoginName = tokenRefresh.NewName;

                return Ok("Tokenhez tartozó név sikeresen frissítve.");
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorDTO() { Id = 91, Message = "Hiba történt az adatok mentése közben" });//--------> hibakezelés
            }
        }
    }
}
