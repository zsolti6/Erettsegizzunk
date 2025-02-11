using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
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
            cbSelectAll.IsChecked = false;
        }

        private void cbSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Theme theme in themes)
            {
                theme.IsSelected = true;
            }
            dgTemaAdatok.Items.Refresh();
        }

        private void cbSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Theme theme in themes)
            {
                theme.IsSelected = false;
            }
            dgTemaAdatok.Items.Refresh();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            Close();
        }

        private async void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (Theme theme in themes)
            {
                if (theme.IsSelected)
                {
                    ids.Add(theme.Id);
                }
            }

            if (ids.Count < 1)
            {
                MessageBoxes.CustomMessageOk("Kérem jelöljön ki legalább egy törlésre szánt elemet!");
                return;
            }

            MessageBoxes.CustomMessageOk(await _apiService.DeletTantargy(new TantargyDeleteDTO() { Ids = ids, Token = user.Token }));
            RefreshUi();
        }

        private void btnUjTantagyFelvitele_Click(object sender, RoutedEventArgs e)
        {
            UjTantargy ujTantargy = new UjTantargy(user);
            ujTantargy.ShowDialog();
            RefreshUi();
        }

        private async void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            //string message = await _apiService.PutTantargyak(new TantargyPutDTO() { subjects = subjects, Token = user.Token });
            //MessageBoxes.CustomMessageOk(message);
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
