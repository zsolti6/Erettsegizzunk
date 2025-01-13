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

namespace ErettsegizzunkAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser user = new LoggedUser();
            try
            {
                user = await _apiService.Login(nev.Text, jelszo.Password);
                if (user.Permission == -1)
                {
                    MessageBoxes.CustomError("Hibás név - jelszó páros!");
                }

                if (user.Permission == -2)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(user.Name,"Hiba");
                return;
            }
            MenuWindow menuWindow = new MenuWindow(user);
            Close();
            menuWindow.ShowDialog();
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
    }
}