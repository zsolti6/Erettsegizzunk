using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for UjFelhasznalo.xaml
    /// </summary>
    public partial class UjFelhasznalo : Window
    {
        private readonly ApiService _apiService;
        public UjFelhasznalo()
        {
            InitializeComponent();
            _apiService = new ApiService();
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
                User newUser = new User()
                {
                    LoginName = tbFelhasznev.Text,
                    Email = tbEmail.Text,
                    PermissionId = cbJogosultsag.SelectedIndex + 1,
                    Active = false,
                    Newsletter = false,
                    Salt = salt,
                    Hash = MainWindow.CreateSHA256(tbJelszoMegint.Password + salt)
                };
                ret = await _apiService.PostFelhasznalo(newUser);
            }
            catch (Exception ex)
            {
                MessageBoxes.CustomMessageOk(ex.Message, "Hiba");
                return;
            }
            MessageBoxes.CustomMessageOk(ret);
            Close();
        }

        private bool Ellonorzes()
        {
            if (tbFelhasznev.Text == string.Empty || tbEmail.Text == string.Empty || tbJelszo.Password == string.Empty || tbJelszoMegint.Password == string.Empty)
            {
                MessageBoxes.CustomError("Ne hagyj üresen mező");
                return false;
            }

            if (tbJelszo.Password != tbJelszoMegint.Password)
            {
                MessageBoxes.CustomError("A két jelszó nem egyezik!");
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
