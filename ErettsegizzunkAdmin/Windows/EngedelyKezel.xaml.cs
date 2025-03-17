using ErettsegizzunkAdmin.CustomMessageBoxes;
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
    public partial class EngedelyKezel : Window
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
        private List<Permission> permissions = new List<Permission>();
        private readonly ApiService _apiService;
        public EngedelyKezel(LoggedUserDTO user)
        {
            InitializeComponent();
            this.user = user;
            _apiService = new ApiService();
            RefreshUi();
        }

        private async void RefreshUi()
        {
            permissions = await _apiService.GetPermessionskAsync(user.Token);
            dgEngedelyekAdatok.ItemsSource = permissions;
            cbSelectAll.IsChecked = false;
        }

        private void cbSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Permission permission in permissions)
            {
                permission.IsSelected = true;
            }
            dgEngedelyekAdatok.Items.Refresh();
        }

        private void cbSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Permission permission in permissions)
            {
                permission.IsSelected = false;
            }
            dgEngedelyekAdatok.Items.Refresh();
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

            foreach (Permission permission in permissions)
            {
                if (permission.IsSelected)
                {
                    ids.Add(permission.Id);
                }
            }

            if (ids.Count < 1)
            {
                MessageBoxes.CustomMessageOk("Kérem jelöljön ki legalább egy törlésre szánt elemet!");
                return;
            }

            MessageBoxResult result = MessageBoxes.CustomQuestion("Biztosan törölni akarja a kijelölt eleme(ke)t?");

            if (result == MessageBoxResult.Cancel)
            {
                MessageBoxes.CustomMessageOk("Törlés megszakítva");
                return;
            }

            await _apiService.DeletPermission(new ParentDeleteDTO() { Ids = ids, Token = user.Token });
            RefreshUi();
        }

        private void btnUjEngedelyFelvitele_Click(object sender, RoutedEventArgs e)
        {
            UjPermission ujPermission = new UjPermission(user);
            ujPermission.ShowDialog();
            RefreshUi();
        }

        private async void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            await _apiService.PutPermssion(new PutPermissionDTO() { Permissions = permissions, Token = user.Token });
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
