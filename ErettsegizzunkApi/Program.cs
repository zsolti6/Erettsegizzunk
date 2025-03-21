using ErettsegizzunkApi.Controllers;
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using DotNetEnv;
using System.Security.Cryptography.X509Certificates;

namespace ErettsegizzunkApi
{
    //Scaffold-DbContext "SERVER=erettsegizzunk2.mysql.database.azure.com;PORT=3306;DATABASE=erettsegizzunk;USER=ErettsegiAdmin;PASSWORD=3rettsegi-4dmin;SSL MODE=required;" mysql.entityframeworkcore -outputdir Models -f
    public static class Program
    {
        public static Dictionary<string, User> LoggedInUsers = new Dictionary<string, User>();
        public static string ftpUrl = "";
        public static string ftpUserName = "";
        public static string ftpPassword = "";
        private static string email = "";
        private static string emailPassword = "";
        private static string smtp = "";

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
            mail.From = new MailAddress(email);
            mail.To.Add(mailAddressTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(email, emailPassword);

            SmtpServer.EnableSsl = true;

            await SmtpServer.SendMailAsync(mail);

        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<RecaptchaService>();
            services.AddScoped<RecaptchaService>();
            services.AddScoped<RegistryController>();
            services.AddScoped<ThemesController>();
        }


        public static void Main(string[] args)
        {
            Env.Load();

            string certBase64 = Env.GetString("CERT_PFX_BASE64");
            string certPassword = Env.GetString("CERT_PASSWORD");
            string port = Env.GetString("PORT") ?? "10000";
            string dbConnection = Env.GetString("CONNECTION_STRING");
            string apiKey = Env.GetString("SECRET_KEY");
            ftpUrl = Env.GetString("FTP_URL");
            ftpUserName = Env.GetString("FTP_USERNAME");
            ftpPassword = Env.GetString("FTP_PASSWORD");
            email = Env.GetString("EMAIL");
            emailPassword = Env.GetString("EMAIL_PASSWORD");
            smtp = Env.GetString("SMTP");

            //vegen kiszedni
            LoggedInUsers["token"] = new User
            {
                Permission = new Permission { Level = 9 }
            };

            var builder = WebApplication.CreateBuilder(args);
            
            if (!string.IsNullOrEmpty(certBase64))//Ki venni ha kell swagger
            {
                var certBytes = Convert.FromBase64String(certBase64);
                var certificate = new X509Certificate2(certBytes, certPassword, X509KeyStorageFlags.MachineKeySet);

                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(int.Parse(port));
                });
            }
            else
            {
                builder.WebHost.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(5000); // Fallback HTTP
                });
            }
            
            builder.Configuration["ConnectionStrings:DefaultConnection"] = dbConnection;
            builder.Configuration["ApiSettings:SecretKey"] = apiKey;

            ConfigureServices(builder.Services);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register DbContext
            builder.Services.AddDbContext<ErettsegizzunkContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || true)
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