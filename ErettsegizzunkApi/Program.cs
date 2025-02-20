using ErettsegizzunkApi.Controllers;
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace ErettsegizzunkApi
{
    //Scaffold-DbContext "SERVER=localhost;PORT=3306;DATABASE=erettsegizzunk;USER=root;PASSWORD=;SSL MODE=none;" mysql.entityframeworkcore -outputdir Models -f
    public static class Program
    {
        //public static int SaltLength = 64;
        public static Dictionary<string, User> LoggedInUsers = new Dictionary<string, User>();
        public static string ftpUrl = "ftp.nethely.hu";
        public static string ftpUserName = "erettsegizzunk";
        public static string ftpPassword = "Eretsegizzunk_Ftp_2024";


        public static string GenerateSalt(int SaltLength = 64)
        {
            Random random = new Random();
            string karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string salt = "";
            for (int i = 0; i < SaltLength; i++)
            {
                salt += karakterek[random.Next(karakterek.Length)];
            }
            return salt;
        }

        public static string CreateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public static async System.Threading.Tasks.Task SendEmail(string mailAddressTo, string subject, string body, bool isHtml = false)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("noreplyerettsegizzunk@gmail.com");
            mail.To.Add(mailAddressTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            /*System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment("");
            mail.Attachments.Add(attachment);*/

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("noreplyerettsegizzunk@gmail.com", "oshelhrvotaqkuej");

            SmtpServer.EnableSsl = true;

            await SmtpServer.SendMailAsync(mail);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<RecaptchaService>();
            services.AddScoped<RecaptchaService>();
        }



        public static void Main(string[] args)
        {
            //vegen kiszedni
            LoggedInUsers["token"] = new User
            {
                Permission = new Permission { Level = 9 }
            };


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient<RecaptchaService>();
            builder.Services.AddScoped<RecaptchaService>();

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5000); // HTTP
                options.ListenAnyIP(7066, listenOptions =>
                {
                    listenOptions.UseHttps(); // HTTPS
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register DbContext
            builder.Services.AddDbContext<ErettsegizzunkContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register controllers
            builder.Services.AddControllers();
            builder.Services.AddScoped<UserStatisticsController>(); //statisztika miatt kell

            builder.Services.AddControllers();
            builder.Services.AddScoped<RegistryController>(); //statisztika miatt kell

            var app = builder.Build();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors("AllowReactApp");

            app.Run();
        }
    }
}