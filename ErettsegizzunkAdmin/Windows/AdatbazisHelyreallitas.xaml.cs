using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for AdatbazisHelyreallitas.xaml
    /// </summary>
    public partial class AdatbazisHelyreallitas : Window
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

        private LoggedUserDTO user;
        private List<GetBackupFileNamesDTO> fileNames;
        private ApiService _apiService;
        public AdatbazisHelyreallitas(LoggedUserDTO user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            fileNames = new List<GetBackupFileNamesDTO>();
            this.user = user;
            RefreshUi();
        }

        private async void RefreshUi()
        {
            fileNames = new List<GetBackupFileNamesDTO>();
            fileNames = await _apiService.GetFileNames(user.Token);
            dgAdatok.ItemsSource = null;
            dgAdatok.ItemsSource = fileNames;
            dgAdatok.Items.Refresh();
        }

        private async void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBoxes.CustomQuestion($"Biztosan készíteni akar egy biztonsági mentést?");

            if (result == MessageBoxResult.Cancel)
            {
                MessageBoxes.CustomMessageOk("Biztonsági mentés megszakítva!");
                return;
            }

            await _apiService.Backup(user.Token);
            RefreshUi();
        }

        private async void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            List<string> betolt = fileNames.Where(x => x.IsSelected).Select(x => x.FileName).ToList();

            if (betolt.Count > 1)
            {
                MessageBoxes.CustomMessageOk("Egyszerre csak egy visszaállítandó file lehet kijeölve!", "Figyelem");
                return;
            }

            if (betolt.Count < 1)
            {
                MessageBoxes.CustomMessageOk("Legalább egy visszaállítandó filet ki kell kijeölni!", "Figyelem");
                return;
            }

            MessageBoxResult result = MessageBoxes.CustomQuestion($"Biztosan vissza akarja állítani a {betolt[0]} biztonsági mentést?");

            if (result == MessageBoxResult.Cancel)
            {
                MessageBoxes.CustomMessageOk("Visszaállítás megszakítva!");
                return;
            }

            await _apiService.Restore(new BackupRestoreDTO() { Token = user.Token, FileName = betolt[0]});
            RefreshUi();
        }

        private void btnVissza_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
