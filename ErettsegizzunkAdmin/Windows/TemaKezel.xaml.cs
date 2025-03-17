using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Models;
using ErettsegizzunkAdmin.Services;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for TemaKezel.xaml
    /// </summary>
    public partial class TemaKezel : Window
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

        private LoggedUserDTO user = new LoggedUserDTO();
        private List<Theme> themes = new List<Theme>();
        private readonly ApiService _apiService;
        public TemaKezel(LoggedUserDTO user)
        {
            InitializeComponent();
            this.user = user;
            _apiService = new ApiService();
            RefreshUi();
        }

        private async void RefreshUi()
        {
            themes = await _apiService.GetTemakAsync();
            dgTemaAdatok.ItemsSource = themes;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            Close();
        }

        private void btnUjTemaFelvitele_Click(object sender, RoutedEventArgs e)
        {
            UjTema ujTema = new UjTema(user);
            ujTema.ShowDialog();
            RefreshUi();
        }

        private async void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            await _apiService.PutTemak(new PutThemeDTO() { Themes = themes, Token = user.Token });
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }
    }
}
