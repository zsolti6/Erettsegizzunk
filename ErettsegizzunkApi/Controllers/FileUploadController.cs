using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        [Route("{uId},{type}")]//tesztelni postmanbe lehet csak

        [HttpPost]

        public async Task<IActionResult> OnPostAsync(string uId, string type)
        {
            //if (Program.LoggedInUsers.ContainsKey(uId))
            if(true)
            {
                try
                {
                    var httpRequest = Request.Form;
                    var postedFile = httpRequest.Files[0];
                    string fileName = postedFile.FileName;
                    string subFolder = "";
                    if (type == "Img")
                    {
                        subFolder = "/Img/";
                    }
                    else
                    {
                        subFolder = "/Documents/";
                    }
                    var url = Program.ftpUrl + subFolder + type + "/" + fileName;
                    Console.WriteLine(url);
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                    request.Credentials = new NetworkCredential(Program.ftpUserName, Program.ftpPassword);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    await using (Stream ftpStream = request.GetRequestStream())
                    {
                        postedFile.CopyTo(ftpStream);
                    }
                    return new JsonResult(fileName);
                }
                catch (Exception ex)
                {
                    return new JsonResult("default.jpg");
                }
            }
            else
            {
                return new JsonResult("Nincs jogosultsága fájlt feltölteni!");
            }

        }


    }
}
