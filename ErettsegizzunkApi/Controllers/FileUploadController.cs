using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentFTP;
using System.Threading.Tasks;
using System.IO;
using ErettsegizzunkApi.DTOs;
using Humanizer;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase //EGÉSZ CONTROLERT ÁTÍRNI TELJESEN MÁSIK HELYRŐL SZEDJÜK LE A KÉPEKET!!!! ====>>> CLOUDINARY
    //#####################Törlés######################
    {

        IWebHostEnvironment _env;
        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

       // [Route("BackEndServer")]
        //[HttpPost]

        private IActionResult FileUploadBackEnd()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                string subFolder = "/Files/";
                var filePath = _env.ContentRootPath + subFolder + fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return Ok(fileName);
            }
            catch (Exception)
            {
                return Ok("default.jpg");
            }
        }

        [Route("FtpServer")]
        [HttpPost]
        public async Task<IActionResult> FileUploadFtp([FromBody] ImageUpload imageUpload)//bugos????
        {
            try
            {
                if (!Program.LoggedInUsers.ContainsKey(imageUpload.Token) || Program.LoggedInUsers[imageUpload.Token].Permission.Level != 9)
                {
                    return BadRequest(new ErrorDTO() { Id = 12, Message = "Hozzáférés megtagadva" });//szam
                }
                using var ms = new MemoryStream();
                await imageUpload.File.CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();

                // (FTP upload logic goes here)

                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return Ok("default.jpg");
            }
        }

        //GetImageFromFtp
        [Route("Image")]
        [HttpPost]
        public async Task<ActionResult<string>> GetImageAsByteArray([FromBody]string fileName)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                string subFolder = "";
                var url = "ftp://ftp.nethely.hu" + subFolder + "/" + fileName;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(Program.ftpUserName, Program.ftpPassword);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                responseStream.CopyTo(ms);
                return Ok(ms.ToArray());
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

    }
}
