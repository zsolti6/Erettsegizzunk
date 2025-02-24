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

        [HttpPost("backup")]
        public async Task<IActionResult> SQLBackupAsync([FromBody] string token)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(token) || Program.LoggedInUsers[token].Permission.Level != 9)
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
                    string filePath = "SQLBackupRestore/backup " + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".sql";
                    await System.Threading.Tasks.Task.Run(() => backup.ExportToFile(filePath));
                    await myConnection.CloseAsync();

                    if (System.IO.File.Exists(filePath))
                    {
                        var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                        //return File(bytes, "text/plain", Path.GetFileName(filePath));
                        return Ok($"Biztonsági mentés \"{Path.GetFileName(filePath)}\" néven sikeresen megtörtént.");
                    }

                    hibaUzenet = "Nincs ilyen file!";
                    byte[] a = System.Text.Encoding.UTF8.GetBytes(hibaUzenet);
                    //return File(a, "text/plain", "Error.txt");
                    return NotFound(new ErrorDTO() { Id = 105, Message = "Hiba történt az adatok mentése közben" });
                }
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 98, Message = "Kapcsolati hiba" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorDTO() { Id = 99, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        [HttpPost("restore")]
        public async Task<IActionResult> SQLRestoreAsync([FromBody] BackupRestoreDTO restoreDTO)
        {
            if (!Program.LoggedInUsers.ContainsKey(restoreDTO.Token) || Program.LoggedInUsers[restoreDTO.Token].Permission.Level != 9)
            {
                return Unauthorized(new ErrorDTO() { Id = 97, Message = "Hozzáférés megtagadva" });
            }

            try
            {
                string? sqlDataSource = _context.Database.GetConnectionString();
                string filePath = Path.Combine(_env.ContentRootPath, "SQLBackupRestore", restoreDTO.FileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return BadRequest(new ErrorDTO() { Id = 102, Message = "A megadott file nem található" });
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
                        return Ok($"A visszaállítás sikeresen lefutott a visszaállított file:\n\"{Path.GetFileName(filePath)}\"");
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "Mentésfájl sikeresen feltöltve. Az SQL szerver nem érhető el!");//the fuck is this
                    }
                }
            }
            catch (MySqlException)
            {
                return StatusCode(500, new ErrorDTO() { Id = 103, Message = "Kapcsolati hiba" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 104, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }

        [HttpPost("get-backup-names")]
        public ActionResult<IEnumerable<List<string>>> GetBackedUpFiles([FromBody] string token)
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(token) || Program.LoggedInUsers[token].Permission.Level != 9)
                {
                    return Unauthorized(new ErrorDTO() { Id = 108, Message = "Hozzáférés megtagadva" });
                }


                string directoryPath = Path.Combine(_env.ContentRootPath, "SQLBackupRestore/");
                Console.WriteLine("Directory Path: " + directoryPath);

                if (Directory.Exists(directoryPath))
                {
                    return Ok( Directory.GetFiles(directoryPath)
                                         .Select(Path.GetFileName)
                                         .ToList());
                }

                return NotFound(new ErrorDTO() { Id = 106, Message = "A keresett adatok nem találhatóak" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorDTO() { Id = 107, Message = "Hiba történt az adatok lekérdezése közben" });
            }
        }
    }
}
