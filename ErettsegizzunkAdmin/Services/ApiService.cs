﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using ErettsegizzunkApi.Models;
using System.Text;

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

        public async Task<List<Feladatok>> GetFeladatoksAsync(double mettol)
        {
            try
            {
                StringContent content = new StringContent(mettol.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Feladatok/get-sok-feladat",content);
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

        public async Task<string> PostFeladatokFromTxt(List<Feladatok> feladatok)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(feladatok), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Feladatok/post-tobb-feladat", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeletFeladatok(List<int> ids)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(_httpClient.BaseAddress, "erettsegizzunk/Feladatok/delete-feladatok"),
                    Content = content
                };
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                // Ensure success and return the response content
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();

            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
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
            catch (HttpRequestException ex)
            {
                return new List<Tantargyak> { new Tantargyak { Id = -1, Nev = ex.Message } };
            }
        }
    }
}