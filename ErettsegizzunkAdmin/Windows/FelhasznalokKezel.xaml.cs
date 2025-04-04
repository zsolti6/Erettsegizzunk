using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Models;
using ErettsegizzunkAdmin.Services;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for FelhasznalokKezel.xaml
    /// </summary>
    public partial class FelhasznalokKezel : Window
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
        private List<User> felhasznalok = new List<User>();
        private List<FelhasznaloModotsitDTO> modositando = new List<FelhasznaloModotsitDTO>();
        private readonly ApiService _apiService;
        private int pageNumber = 0;
        HowToUseWindow help;

        public FelhasznalokKezel(LoggedUserDTO user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.user = user;
            RefreshUi();
        }

        private async Task<List<string>> SetLists(string token)
        {
            return (await _apiService.GetPermessionskAsync(token)).Select(x => x.Name).ToList();
        }

        private async void RefreshUi(bool oldalKov = false, bool lekerdez = true)
        {
            if (lekerdez)
            {
                felhasznalok = await LoadDatasAsync(felhasznalok.Count == 50 && oldalKov ? felhasznalok[felhasznalok.Count - 1].Id : felhasznalok.Count == 0 ? 0 : felhasznalok[0].Id - 51);
                List<string> jogosultsagok = (await _apiService.GetPermessionskAsync(user.Token))
                        .Select(x => x.Name)
                        .ToList();
                foreach (User item in felhasznalok)
                {
                    item.JogosultsagList = jogosultsagok;
                }
            }

            dgAdatok.ItemsSource = null;
            dgAdatok.ItemsSource = felhasznalok;
            dgAdatok.DataContext = this;
            cbSelectAll.IsChecked = false;
        }

        private async Task<List<User>> LoadDatasAsync(int mettol)
        {
            felhasznalok.Clear();
            List<User> users = await _apiService.GetFelhasznalokAsync(new LoggedUserForCheckDTO() { Id = user.Id, Permission = user.Permission, Token = user.Token });
            if (users is null)
            {
                return new List<User>();
            }
            //btnOldalKov.IsEnabled = feladatoks.Count == 50;//teszt
            return users;
        }

        private void btnOldalKov_Click(object sender, RoutedEventArgs e)
        {
            pageNumber++;
            RefreshUi(true);

            if (pageNumber > 0)
            {
                btnOldalElozo.IsEnabled = true;
            }

            if (felhasznalok.Count < 50)
            {
                (sender as Button).IsEnabled = false;
            }
        }

        private void btnOldalElozo_Click(object sender, RoutedEventArgs e)
        {
            pageNumber--;
            RefreshUi();

            if (pageNumber < 1)
            {
                (sender as Button).IsEnabled = false;
            }

            if (btnOldalKov.IsEnabled == false)
            {
                btnOldalKov.IsEnabled = true;
            }
        }

        private void btnUj_Click(object sender, RoutedEventArgs e)
        {
            UjFelhasznalo ujFelhasznalo = new UjFelhasznalo(felhasznalok[0].JogosultsagList);
            ujFelhasznalo.ShowDialog();
            RefreshUi();
        }

        private async void btnTorol_Click(object sender, RoutedEventArgs e)//megkérdezni h biztos-e
        {
            List<int> ids = new List<int>();

            foreach (User felhasznalo in felhasznalok)
            {
                if (felhasznalo.IsSelected)
                {
                    ids.Add(felhasznalo.Id);
                }
            }

            if (ids.Count < 1)
            {
                MessageBoxes.CustomMessageOk("Kérem jelöljön ki legalább egy törlésre szánt elemet!");
                return;
            }

            MessageBoxResult result = MessageBoxes.CustomQuestion("Biztosan tötölni akarja a kijelölt felhasználó(ka)t?");

            if (result == MessageBoxResult.Cancel)
            {
                MessageBoxes.CustomMessageOk("Törlés megszakítva!");
                return;
            }

            if (ids.Contains(user.Id))
            {
                MessageBoxes.CustomError("A törlendő fiókok tartalmaznak egy admin felhasználói fiókot! Az admin fiók törlése nem lehetséges csak egy másik admin fiókból!");
                return;
            }

            await _apiService.DeletFelhasznalok(new ParentDeleteDTO() { Ids = ids, Token = user.Token });
            RefreshUi();
        }

        private async void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            await _apiService.PutFelhasznalok(new FelhasznaloModotsitDTO() { users = felhasznalok, Token = user.Token });
        }

        private async void btnVissza_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            if (modositando.Count != 0)
            {
                MessageBoxResult result = MessageBoxes.CustomQuestion("Szeretné menteni a módostásokat?");

                if (result == MessageBoxResult.OK)
                {
                    btnModosit_Click(sender, e);
                }
                else
                {
                    MessageBoxes.CustomMessageOk("Módosítások elvetve");
                }
            }
            if (help != null)
            {
                help.Close();
            }
            Close();
        }

        private void cbSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (User felhasznalo in felhasznalok)
            {
                felhasznalo.IsSelected = true;
            }
            dgAdatok.Items.Refresh();
        }

        private void cbSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (User felhasznalo in felhasznalok)
            {
                felhasznalo.IsSelected = false;
            }
            dgAdatok.Items.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {
            help = new HowToUseWindow("FelhasznalokKezel");
            help.Show();
        }
    }
}
