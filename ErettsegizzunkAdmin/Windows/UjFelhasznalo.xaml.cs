using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.Models;
using ErettsegizzunkAdmin.Services;
using System.Windows;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for UjFelhasznalo.xaml
    /// </summary>
    public partial class UjFelhasznalo : Window
    {
        private readonly ApiService _apiService;
        User newUser;
        public UjFelhasznalo(List<string> jogosultsagList)
        {
            InitializeComponent();
            _apiService = new ApiService();
            newUser = new User() { JogosultsagList = jogosultsagList };
            DataContext = newUser;
        }

        private async void UjFelhasznalo_Click(object sender, RoutedEventArgs e)
        {
            if (!Ellonorzes())
            {
                return;
            }

            string ret = string.Empty;

            try
            {
                string salt = MainWindow.GenerateSalt();
                newUser = new User()
                {
                    LoginName = tbFelhasznev.Text,
                    Email = tbEmail.Text,
                    PermissionId = cbJogosultsag.SelectedIndex + 1,
                    Active = false,
                    Newsletter = false,
                    Salt = salt,
                    Hash = MainWindow.CreateSHA256(tbJelszoMegint.Password + salt),
                    SignupDate = DateTime.Now
                };
                await _apiService.PostFelhasznalo(newUser);
            }
            catch (Exception ex)
            {
                MessageBoxes.CustomMessageOk(ex.Message);
                return;
            }
            Close();
        }

        private bool Ellonorzes()
        {
            if (tbFelhasznev.Text == string.Empty || tbEmail.Text == string.Empty || tbJelszo.Password == string.Empty || tbJelszoMegint.Password == string.Empty)
            {
                MessageBoxes.CustomError("Minden mező kitöltése kötelező", "Figyelem");
                return false;
            }

            if (tbJelszo.Password != tbJelszoMegint.Password)
            {
                MessageBoxes.CustomError("A két jelszó nem egyezik!", "Figyelem");
                return false;
            }

            return true;
        }

        private void Megse_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
