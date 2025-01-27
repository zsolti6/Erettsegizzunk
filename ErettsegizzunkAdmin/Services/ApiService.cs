using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using ErettsegizzunkApi.Models;
using System.Text;
using ErettsegizzunkAdmin.DTOs;
using System.Net.Http.Json;
using ErettsegizzunkApi.DTOs;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ErettsegizzunkAdmin.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7066/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //Feladatok
        public async Task<List<ErettsegizzunkApi.Models.Task>> GetFeladatoksAsync(int mettol)
        {
            try
            {
                StringContent content = new StringContent(mettol.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Feladatok/get-sok-feladat",content);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ErettsegizzunkApi.Models.Task>>(responseContent);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");//rendesen kiirni majd
                return null;
            }
        }

        public async Task<string> PostFeladatokFromTxt(List<FeladatokPutPostDTO> feladatok)
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

        public async Task<string> DeletFeladatok(FeladatokDeleteDTO feladatokDeleteDTO)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(feladatokDeleteDTO), Encoding.UTF8, "application/json");

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

        //Tantargyak
        public async Task<List<Subject>> GetTantargyaksAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/erettsegizzunk/Tantargyak");
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Subject>>(responseContent);
            }
            catch (HttpRequestException ex)
            {
                return new List<Subject> { new Subject { Id = -1, Name = ex.Message } };
            }
        }


        //Login - logout
        public async Task<LoggedUserDTO> Login(string name, string password)
        {
            LoggedUserDTO user = new LoggedUserDTO();
            try
            {
                //getsalt
                string formatted = $"\"{name}\"";
                StringContent contentGetSalt = new StringContent(formatted, Encoding.UTF8, "application/json");
                HttpResponseMessage responseGetSalt = await _httpClient.PostAsync("erettsegizzunk/Login/SaltRequest", contentGetSalt);
                responseGetSalt.EnsureSuccessStatusCode();
                string salt = await responseGetSalt.Content.ReadAsStringAsync();

                //login
                string tmpHash = MainWindow.CreateSHA256(password + salt.Replace("\"",""));
                StringContent contentLogin = new StringContent(JsonConvert.SerializeObject(new LoginRequest { LoginName = name, TmpHash = tmpHash}), Encoding.UTF8, "application/json");
                HttpResponseMessage responseLogin = await _httpClient.PostAsync("erettsegizzunk/Login", contentLogin);
                responseGetSalt.EnsureSuccessStatusCode();
                user = await responseLogin.Content.ReadFromJsonAsync<LoggedUserDTO>();

                //saveToken
                StringContent contentToken = new StringContent(JsonConvert.SerializeObject(new AddTokenDTO() { Token = user.Token, UserId = user.Id }), Encoding.UTF8, "application/json");
                HttpResponseMessage responseToken = await _httpClient.PostAsync("erettsegizzunk/Token/add-token", contentToken);
                responseToken.EnsureSuccessStatusCode();
                string response = await responseToken.Content.ReadAsStringAsync();
                App.id = int.Parse(response.Replace("\"", ""));
                return user;
            }
            catch (HttpRequestException ex)
            {
                return user;
            }
            catch(Exception ex)
            {
                return user;
            }
        }

        public async Task<string> LogOut(ModifyToken modifyToken)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(modifyToken), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("erettsegizzunk/Token/kijelentkez", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Kép letöltés
        public async Task<string> GetImage(string imageName)
        {
            try
            {
                string formatted = $"\"{imageName}\"";
                StringContent content = new StringContent(formatted, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/FileUpload/Image", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<BitmapImage> ByteArrayToBitmapImage(string imageName)
        {
            try
            {
                string base64String = await GetImage(imageName);
                byte[] byteArray = Convert.FromBase64String(base64String.Replace("\"",""));
                MemoryStream ms = new MemoryStream(byteArray);
                ms.Position = 0;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                ms.Close();

                return bitmap;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //Felhasználók
        public async Task<List<User>> GetFelhasznalokAsync(LoggedUserForCheckDTO logged /*int mettol*/)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(logged), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/User/get-sok-felhasznalo", content);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(responseContent);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");//rendesen kiirni majd
                return null;
            }
        }

        public async Task<string> PostFelhasznalo(User user)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Registry/regisztracio", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeletFelhasznalok(FelhasznaloTorolDTO felhasznalokDelete)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(felhasznalokDelete), Encoding.UTF8, "application/json");

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(_httpClient.BaseAddress, "erettsegizzunk/User/delete-felhasznalok"),
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
    }
}