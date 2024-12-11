using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using ErettsegizzunkApi.Models;

namespace ErettsegizzunkAdmin.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7066/erettsegizzunk");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Feladatok>> GetFeladatoksAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/erettsegizzunk/Feladatok");
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Feladatok>>(responseContent);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");//rendesen kiirni majd
                return null;
            }
        }

        public async Task<List<Tantargyak>> GetTantargyaksAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/erettsegizzunk/Tantargyak");
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Tantargyak>>(responseContent);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");//rendesen kiirni majd
                return null;
            }
        }
    }
}