using ErettsegizzunkAdmin.Services;
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
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.DTOs;
using System.Drawing;
using System.IO;
using ErettsegizzunkAdmin.CustomMessageBoxes;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        #region Bezaras gomb eltüntetése
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        const uint MF_GRAYED = 0x00000001;
        const uint MF_ENABLED = 0x00000000;
        const uint SC_CLOSE = 0xF060;
        #endregion


        LoggedUserDTO user;
        private readonly ApiService _apiService;
        public MenuWindow(LoggedUserDTO user)
        {
            InitializeComponent();

            _apiService = new ApiService();
            this.user = user;
            lbUdvozles.Content = "Üdv: " + user.Name;
            imgUser.Source = user.ProfilePicture;
        }

        //Feladatok kezelése
        private void feladatKezelese_Click(object sender, RoutedEventArgs e)
        {
            FeladatokKezel feladatok = new FeladatokKezel(user);
            feladatok.Show();
            Close();
        }

        //Feladatok témáinak kezelése
        private void feladatokTemaKezelese_Click(object sender, RoutedEventArgs e)
        {

        }

        //Minden egyéb kezelése
        private void segedKezelese_Click(object sender, RoutedEventArgs e)
        {

        }

        //Felhasználók kezelésére
        private void felhasznalokKezel_Click(object sender, RoutedEventArgs e)
        {
            FelhasznalokKezel felhasznalok = new FelhasznalokKezel(user);
            felhasznalok.Show();
            Close();
        }

        //Kijelentkezés
        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            string uzenet = string.Empty;
            try
            {
                uzenet = await _apiService.LogOut(new ModifyToken()
                {
                    Id = App.id,
                    Aktiv = false,
                    LogOut = DateTime.Now
                }, user.Token);
            }
            catch (Exception)
            {
                uzenet = "Hiba történt a kijelentkezés folyamán.";
            }
            finally
            {
                MessageBoxes.CustomMessageOk(uzenet);
            }            

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }

        private void tantargyKezel_Click(object sender, RoutedEventArgs e)
        {
            TantargyKezel tantargy = new TantargyKezel(user);
            tantargy.Show();
            Close();
        }
    }
}
