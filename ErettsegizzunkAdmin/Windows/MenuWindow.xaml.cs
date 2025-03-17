using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Services;
using System.Runtime.InteropServices;
using System.Windows;
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

        //Adatbázis manuális mentése / visszaállítása
        private void adatbazisBackup_Click(object sender, RoutedEventArgs e)
        {
            AdatbazisHelyreallitas helyreallitas = new AdatbazisHelyreallitas(user);
            helyreallitas.Show();
            Close();
        }

        //Feladatok témáinak kezelése
        private void feladatokTemaKezelese_Click(object sender, RoutedEventArgs e)
        {
            TemaKezel tema = new TemaKezel(user);
            tema.Show();
            Close();
        }

        //Engedélyek kezelése
        private void engedelyekkezel_Click(object sender, RoutedEventArgs e)
        {
            EngedelyKezel engedely = new EngedelyKezel(user);
            engedely.Show();
            Close();
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
            await _apiService.LogOut(user.Token);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void tantargyKezel_Click(object sender, RoutedEventArgs e)
        {
            TantargyKezel tantargy = new TantargyKezel(user);
            tantargy.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }
    }
}
