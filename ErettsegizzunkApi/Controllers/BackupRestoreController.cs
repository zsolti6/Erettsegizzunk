﻿using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class BackupRestoreController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public BackupRestoreController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Route("Backup/{token},{fileName}")]
        [HttpGet]
        public async Task<IActionResult> SQLBackupAsync(string token, string fileName)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                string hibaUzenet = "";
                using (var context = new ErettsegizzunkContext())
                {
                    string? sqlDataSource = context.Database.GetConnectionString();
                    MySqlCommand command = new MySqlCommand();
                    MySqlBackup backup = new MySqlBackup(command);
                    using (MySqlConnection myConnection = new MySqlConnection(sqlDataSource))
                    {
                        try
                        {
                            command.Connection = myConnection;
                            await myConnection.OpenAsync();
                            var filePath = "SQLBackupRestore/" + fileName;
                            await System.Threading.Tasks.Task.Run(() => backup.ExportToFile(filePath));
                            await myConnection.CloseAsync();
                            if (System.IO.File.Exists(filePath))
                            {
                                var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                                return File(bytes, "text/plain", Path.GetFileName(filePath));
                            }
                            else
                            {
                                hibaUzenet = "Nincs ilyen file!";
                                byte[] a = System.Text.Encoding.UTF8.GetBytes(hibaUzenet);
                                return File(a, "text/plain", "Error.txt");
                            }

                        }
                        catch (Exception ex)
                        {
                            return BadRequest(new { error = ex.Message });
                        }
                    }
                }
            }
            else
            {
                return Unauthorized("Nincs bejelentkezve/jogosultsága!");
            }
        }

        [Route("Restore/{token}")]
        [HttpPost]
        public async Task<IActionResult> SQLRestoreAsync(string token)
        {
            if (Program.LoggedInUsers.ContainsKey(token) && Program.LoggedInUsers[token].Permission.Level == 9)
            {
                try
                {
                    var context = new ErettsegizzunkContext();
                    string? sqlDataSource = context.Database.GetConnectionString();
                    var httpRequest = Request.Form;
                    var postedFile = httpRequest.Files[0];
                    string fileName = postedFile.FileName;
                    var filePath = Path.Combine(_env.ContentRootPath, "SQLBackupRestore", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await postedFile.CopyToAsync(stream);
                    }

                    MySqlCommand command = new MySqlCommand();
                    MySqlBackup restore = new MySqlBackup(command);
                    using (MySqlConnection mySqlConnection = new MySqlConnection(sqlDataSource))
                    {
                        try
                        {
                            command.Connection = mySqlConnection;
                            await mySqlConnection.OpenAsync();
                            await System.Threading.Tasks.Task.Run(() => restore.ImportFromFile(filePath));
                            System.IO.File.Delete(filePath);
                            return Ok("A visszaállítás sikeresen lefutott.");
                        }
                        catch
                        {
                            return StatusCode(500, "Mentésfájl sikeresen feltöltve. Az SQL szerver nem érhető el!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = "Nincs kiválasztva mentés fájl!", details = ex.Message });
                }
            }
            else
            {
                return Unauthorized("Nincs bejelentkezve/jogosultsága!");
            }
        }


    }
}
