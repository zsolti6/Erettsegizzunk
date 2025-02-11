using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ErettsegizzunkApi.Services
{
    public class RecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _recaptchaSecretKey;

        public RecaptchaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _recaptchaSecretKey = configuration["GoogleRecaptcha:SecretKey"];
        }

        public async Task<bool> VerifyRecaptchaAsync(string token)
        {
            var response = await _httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSecretKey}&response={token}",
                null
            );

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(jsonResponse);
            return jsonDoc.RootElement.GetProperty("success").GetBoolean();
        }
    }
}