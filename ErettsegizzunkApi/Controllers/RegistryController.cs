﻿using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        [HttpPost]//tesztelni erősen

        public async Task<IActionResult> Registry(User user)
        {
            using (var cx = new ErettsegizzunkContext())
            {
                try
                {
                    if (cx.Users.FirstOrDefault(f => f.LoginNev == user.LoginNev) != null)
                    {
                        return Ok("Már létezik ilyen felhasználónév!");
                    }
                    if (cx.Users.FirstOrDefault(f => f.Email == user.Email) != null)
                    {
                        return Ok("Ezzel az e-mail címmel már regisztráltak!");
                    }
                    user.PermissionId = 1;
                    user.Active = false;
                    user.Hash = Program.CreateSHA256(user.Hash);
                    await cx.Users.AddAsync(user);
                    await cx.SaveChangesAsync();

                    Program.SendEmail(user.Email, "Regisztráció", $"https://localhost:7066/erettsegizzunk/Registry?felhasznaloNev={user.LoginNev}&email={user.Email}");

                    return Ok("Sikeres regisztráció. Fejezze be a regisztrációját az e-mail címére küldött link segítségével!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]

        public async Task<IActionResult> EndOfTheRegistry(string felhasznaloNev, string email)
        {
            using (var cx = new ErettsegizzunkContext())
            {
                try
                {
                    User user = await cx.Users.FirstOrDefaultAsync(f => f.LoginNev == felhasznaloNev && f.Email == email);
                    if (user == null)
                    {
                        return Ok("Sikertelen a regisztráció befejezése!");
                    }
                    else
                    {
                        user.Active = true;
                        cx.Users.Update(user);
                        await cx.SaveChangesAsync();
                        return Ok("A regisztráció befejezése sikeresen megtörtént.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }   
        }
    }
}