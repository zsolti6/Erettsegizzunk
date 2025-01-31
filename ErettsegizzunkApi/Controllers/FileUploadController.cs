using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {

        IWebHostEnvironment _env;
        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Route("BackEndServer")]
        [HttpPost]

        public IActionResult FileUploadBackEnd()
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

        public async Task<IActionResult> FileUploadFtp()//from body?
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                string subFolder = "";

                var url = "ftp://ftp.nethely.hu" + subFolder + "/" + fileName;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.Credentials = new NetworkCredential(Program.ftpUserName, Program.ftpPassword);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                await using (Stream ftpStream = request.GetRequestStream())
                {
                    postedFile.CopyTo(ftpStream);
                }
                return Ok(fileName);

            }
            catch (Exception ex)
            {
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
