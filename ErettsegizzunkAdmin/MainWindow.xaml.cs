using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkAdmin.Windows;
using ErettsegizzunkApi.Models;
using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkApi.DTOs;
using MaterialDesignThemes.Wpf;

namespace ErettsegizzunkAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private const int SALT_LENGTH = 64;
        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoggedUserDTO user = new LoggedUserDTO();
            try
            {
                user = await _apiService.Login(nev.Text, jelszo.Password);

                if (user.Permission == -1)
                {
                    return;
                }

                if (user.Permission != 2)
                {
                    MessageBoxes.CustomError(new ErrorDTO(500, "Hozzáférés megtagadva").ToString(), "Figyelem");
                    return;
                }

                user.ProfilePicture = await _apiService.ByteArrayToBitmapImage(user.ProfilePicturePath);
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(501, "Kapcsolati hiba").ToString());
                return;
            }

            MenuWindow menuWindow = new MenuWindow(user);
            menuWindow.Show();
            Close();
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

        public static string GenerateSalt()
        {
            Random random = new Random();
            string karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string salt = "";
            for (int i = 0; i < SALT_LENGTH; i++)
            {
                salt += karakterek[random.Next(karakterek.Length)];
            }
            return salt;
        }

    }
}