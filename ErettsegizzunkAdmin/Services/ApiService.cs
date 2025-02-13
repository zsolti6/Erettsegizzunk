using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Windows.Media.Imaging;
using Task = System.Threading.Tasks.Task;

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

        #region Feladatok
        public async Task<List<ErettsegizzunkApi.Models.Task>> GetFeladatoksAsync(int mettol)
        {
            try
            {
                StringContent content = new StringContent(mettol.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Feladatok/get-sok-feladat", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ErettsegizzunkApi.Models.Task>>(responseContent);
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
                return new List<ErettsegizzunkApi.Models.Task>();
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(502, "Kapcsolati hiba").ToString());
                return new List<ErettsegizzunkApi.Models.Task>();
            }
        }

        public async Task PostFeladatokFromTxt(List<FeladatokPutPostDTO> feladatok)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(feladatok), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Feladatok/post-tobb-feladat", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(503, "Kapcsolati hiba").ToString());
            }
        }

        public async Task DeletFeladatok(FeladatokDeleteDTO feladatokDeleteDTO)
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

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));

            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(504, "Kapcsolati hiba").ToString());
            }
        }

        public async Task PutFeladatok(FeladatokPutPostDTO feladatok)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(feladatok), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("erettsegizzunk/Feladatok/put-egy-feladat", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(505, "Kapcsolati hiba").ToString());
            }
        }
        #endregion

        #region Tantargyak
        public async Task<List<Subject>> GetTantargyaksAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("erettsegizzunk/Tantargyak/get-tantargyak");

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Subject>>(responseContent);
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
                return new List<Subject>();
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(501, "Kapcsolati hiba").ToString());
                return new List<Subject>();
            }
        }

        public async Task PutTantargyak(TantargyPutDTO tantargy)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(tantargy), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("erettsegizzunk/Tantargyak/put-tantargy", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(501, "Kapcsolati hiba").ToString());
            }
        }

        public async Task PostTantargy(TantargyDTO tantargy)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(tantargy), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Tantargyak/post-tantargy", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(501, "Kapcsolati hiba").ToString());
            }
        }

        public async Task DeletTantargy(TantargyDeleteDTO tantargy)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(tantargy), Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(_httpClient.BaseAddress, "erettsegizzunk/Tantargyak/delete-tantargyak"),
                    Content = content
                };
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));

            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(501, "Kapcsolati hiba").ToString());
            }
        }
        #endregion

        #region Téma
        public async Task<List<Theme>> GetTemakAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("erettsegizzunk/Themes/get-temak");

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Theme>>(responseContent);
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
                return new List<Theme>();
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(501, "Kapcsolati hiba").ToString());
                return new List<Theme>();
            }
        }

        #endregion

        #region Login - logout
        public async Task<LoggedUserDTO> Login(string name, string password)
        {
            LoggedUserDTO user = new LoggedUserDTO();
            try
            {
                //getsalt
                string formatted = $"\"{name}\"";
                StringContent contentGetSalt = new StringContent(formatted, Encoding.UTF8, "application/json");
                HttpResponseMessage responseGetSalt = await _httpClient.PostAsync("erettsegizzunk/Login/SaltRequest", contentGetSalt);

                if (!responseGetSalt.IsSuccessStatusCode)
                {
                    string error = await responseGetSalt.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }
                string salt = JsonConvert.DeserializeObject<string>(await responseGetSalt.Content.ReadAsStringAsync());

                //login
                string tmpHash = MainWindow.CreateSHA256(password + salt);//.Replace("\"","")
                StringContent contentLogin = new StringContent(JsonConvert.SerializeObject(new LoginRequest { LoginName = name, TmpHash = tmpHash }), Encoding.UTF8, "application/json");
                HttpResponseMessage responseLogin = await _httpClient.PostAsync("erettsegizzunk/Login", contentLogin);

                if (!responseLogin.IsSuccessStatusCode)
                {
                    string error = await responseLogin.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }
                return await responseLogin.Content.ReadFromJsonAsync<LoggedUserDTO>();
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
                return user;
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(506, "Kapcsolati hiba").ToString());
                return user;
            }
        }

        public async Task LogOut(string token)
        {
            try
            {
                string formatted = $"\"{token}\"";
                StringContent content = new StringContent(formatted, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Logout", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(507, "Kapcsolati hiba").ToString());
            }
        }
        #endregion

        #region Kép letöltés
        public async Task<string> GetImage(string imageName)
        {
            try
            {
                string formatted = $"\"{imageName}\"";
                StringContent content = new StringContent(formatted, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/FileUpload/Image", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
                return string.Empty;
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(508, "Kapcsolati hiba").ToString());
                return string.Empty;
            }
        }

        public async Task<BitmapImage> ByteArrayToBitmapImage(string imageName)
        {
            try
            {
                string base64String = await GetImage(imageName);
                byte[] byteArray = Convert.FromBase64String(base64String.Replace("\"", ""));
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
            catch (Exception)
            {
                MessageBoxes.CustomError("Hiba történt a profilkép betöltése közben");
                return null;
            }

        }
        #endregion

        #region Felhasználók
        public async Task<List<User>> GetFelhasznalokAsync(LoggedUserForCheckDTO logged /*int mettol*/)//50 lekérdezése max
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(logged), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/User/get-sok-felhasznalo", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(responseContent);
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
                return new List<User>();
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(509, "Kapcsolati hiba").ToString());
                return new List<User>();
            }
        }

        public async Task PostFelhasznalo(User user)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/Registry/regisztracio", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(510, "Kapcsolati hiba").ToString());
            }
        }

        public async Task DeletFelhasznalok(FelhasznaloTorolDTO felhasznalokDelete)
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

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));

            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(511, "Kapcsolati hiba").ToString());
            }
        }

        public async Task PutFelhasznalok(FelhasznaloModotsitDTO users)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("erettsegizzunk/User/felhasznalok-modosit", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(512, "Kapcsolati hiba").ToString());
            }
        }
        #endregion

        #region Adatbazis mentes, visszallitas

        public async Task Backup(string token)//void??
        {
            try
            {
                string formatted = $"\"{token}\"";
                StringContent content = new StringContent(formatted, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/BackupRestore/backup", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(517, "Kapcsolati hiba").ToString());
            }
        }

        public async Task Restore(BackupRestoreDTO restore)//void??
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(restore), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/BackupRestore/restore", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }

                MessageBoxes.CustomMessageOk(JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()));
            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(518, "Kapcsolati hiba").ToString());
            }
        }

        public async Task<List<GetBackupFileNamesDTO>> GetFileNames(string token)
        {
            try
            {
                string formatted = $"\"{token}\"";
                StringContent content = new StringContent(formatted, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("erettsegizzunk/BackupRestore/get-backup-names", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new ErrorDTO(error);
                }
                List<string> files = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());

                List<GetBackupFileNamesDTO> getBackups = new List<GetBackupFileNamesDTO>();
                foreach (string item in files)
                {
                    getBackups.Add(new GetBackupFileNamesDTO() { FileName = item });
                }
                return getBackups.OrderByDescending(x => x.FileName).ToList();

            }
            catch (ErrorDTO er)
            {
                ErrorDTO error = JsonConvert.DeserializeObject<ErrorDTO>(er.Message);
                MessageBoxes.CustomError(error.ToString());
                return new List<GetBackupFileNamesDTO>();
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(517, "Kapcsolati hiba").ToString());
                return new List<GetBackupFileNamesDTO>();
            }
        }

        #endregion
    }
}