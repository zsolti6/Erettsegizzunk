using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
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
        private readonly ErettsegizzunkContext _context;

        public BackupRestoreController(IWebHostEnvironment env)
        {
            _env = env;
            _context = new ErettsegizzunkContext();
        }

        [Route("Backup")]
        [HttpPost]
        public async Task<IActionResult> SQLBackupAsync([FromBody] BackupDTO backupDTO)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(backupDTO.Token) || Program.LoggedInUsers[backupDTO.Token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 97, Message = "Hozzáférés megtagadva" });
                }

                string hibaUzenet = "";

                string? sqlDataSource = _context.Database.GetConnectionString();
                MySqlCommand command = new MySqlCommand();
                MySqlBackup backup = new MySqlBackup(command);
                using (MySqlConnection myConnection = new MySqlConnection(sqlDataSource))
                {

                    command.Connection = myConnection;
                    await myConnection.OpenAsync();
                    var filePath = "SQLBackupRestore/" + backupDTO.FileName;
                    await System.Threading.Tasks.Task.Run(() => backup.ExportToFile(filePath));
                    await myConnection.CloseAsync();
                    if (System.IO.File.Exists(filePath))
                    {
                        var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                        return File(bytes, "text/plain", Path.GetFileName(filePath));
                    }
                    hibaUzenet = "Nincs ilyen file!";
                    byte[] a = System.Text.Encoding.UTF8.GetBytes(hibaUzenet);
                    return File(a, "text/plain", "Error.txt");

                }
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 98, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 99, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        [Route("Restore")]
        [HttpPost]
        public async Task<IActionResult> SQLRestoreAsync([FromBody] string token)
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
