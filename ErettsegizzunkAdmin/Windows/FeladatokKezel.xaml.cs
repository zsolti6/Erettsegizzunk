using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Task = ErettsegizzunkApi.Models.Task;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for FeladatokKezel.xaml
    /// </summary>
    public partial class FeladatokKezel : Window
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

        private readonly ApiService _apiService;
        private int pageNumber = 0;
        private List<Task> feladatok = new List<Task>();
        private LoggedUserDTO user;

        public FeladatokKezel(LoggedUserDTO user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.user = user;
            RefreshUi();
        }

        private async void RefreshUi(bool oldalKov = false, bool lekerdez = true)
        {
            if (lekerdez)
            {
                feladatok = await LoadDatasAsync(feladatok.Count == 50 && oldalKov ? feladatok[feladatok.Count - 1].Id : feladatok.Count == 0 ? 0 : feladatok[0].Id - 51);//teszt de elv megy
            }
            dgFeladatAdatok.ItemsSource = null;
            dgFeladatAdatok.ItemsSource = feladatok;
            dgFeladatAdatok.DataContext = this;
            cbSelectAll.IsChecked = false;
        }

        private async Task<List<Task>> LoadDatasAsync(int mettol)
        {
            feladatok.Clear();
            List<Task> feladatoks = await _apiService.GetFeladatoksAsync(mettol);
            /*if (feladatoks is null)
            {
                //MessageBoxes.CustomError(new ErrorDTO(513,"Hiba történt az adatok lekérdezése közben").ToString());
                return new List<ErettsegizzunkApi.Models.Task>();
            }*/
            btnOldalKov.IsEnabled = feladatoks.Count == 50;//teszt
            return feladatoks;
        }

        /// <summary>
        /// Új feladat feltöltése tabulátorral tagolt txt-ből, kép oszlop megléte nem kötelező
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUjAdatokTxtbol_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = "*txt",
                Filter = "txt|*.txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                MessageBoxResult result = MessageBoxes.CustomQuestion($"Biztosan fel akarod tölteni a  \"{openFileDialog.FileName.Split("\\").ToList().Last()}\"  adatait?");
                if (result == MessageBoxResult.Cancel)
                {
                    MessageBoxes.CustomMessageOk("Feltöltés megszakítva!");
                    return;
                }

                try
                {
                    List<FeladatokPutPostDTO> feladatoks = new List<FeladatokPutPostDTO>();
                    StreamReader reader = new StreamReader(openFileDialog.FileName);
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string teszt = reader.ReadLine();
                        string[] sor = teszt.Split("\t");
                        if (sor.Length == 8)
                        {
                            feladatoks.Add(new FeladatokPutPostDTO { Leiras = sor[0], Szoveg = sor[1], Megoldasok = sor[2], Helyese = sor[3], TantargyId = int.Parse(sor[4]), TipusId = int.Parse(sor[5]), SzintId = int.Parse(sor[6]), KepNev = sor[7] });
                        }
                        else
                        {
                            feladatoks.Add(new FeladatokPutPostDTO { Leiras = sor[0], Szoveg = sor[1], Megoldasok = sor[2], Helyese = sor[3], TantargyId = int.Parse(sor[4]), TipusId = int.Parse(sor[5]), SzintId = int.Parse(sor[6]) });
                        }

                        if (feladatoks.Count == 1)
                        {
                            feladatoks[0].Token = user.Token;
                        }
                    }
                    reader.Close();
                    await _apiService.PostFeladatokFromTxt(feladatoks);
                    RefreshUi();
                }
                catch (ErrorDTO ex)
                {
                    MessageBoxes.CustomError(ex.ToString());
                    return;
                }
                catch (FileNotFoundException)
                {
                    MessageBoxes.CustomError(new ErrorDTO(514, "A megadott file nem található").ToString());
                    return;
                }
                catch (Exception)
                {
                    MessageBoxes.CustomError(new ErrorDTO(515, "Hiba történt az adatok mentése közben").ToString());
                    return;
                }
            }
        }

        private void btnOldalKov_Click(object sender, RoutedEventArgs e)
        {
            pageNumber++;
            RefreshUi(true);

            if (pageNumber > 0)
            {
                btnOldalElozo.IsEnabled = true;
            }

            if (feladatok.Count < 50)
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

        private void cbSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Task feladat in feladatok)
            {
                feladat.IsSelected = true;
            }
            dgFeladatAdatok.Items.Refresh();
        }

        private void cbSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Task feladat in feladatok)
            {
                feladat.IsSelected = false;
            }
            dgFeladatAdatok.Items.Refresh();
        }

        private async void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (Task feladat in feladatok)
            {
                if (feladat.IsSelected)
                {
                    ids.Add(feladat.Id);
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

            await _apiService.DeletFeladatok(new FeladatokDeleteDTO() { Ids = ids, Token = user.Token });
            RefreshUi();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            Close();
        }

        private void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (Task feladat in feladatok)
            {
                if (feladat.IsSelected)
                {
                    ids.Add(feladat.Id);
                }
            }

            if (ids.Count == 0)
            {
                MessageBoxes.CustomError("Kérem válassza ki a módosítani kívánt feladatot!");
                return;
            }

            if (ids.Count > 1)
            {
                MessageBoxes.CustomError("Egyszerre csak egy feladat módosítása lehetséges!");
                return;
            }

            FeladatModosit feladatModosit = new FeladatModosit(feladatok.Find(x => x.Id == ids[0]), user);
            feladatModosit.ShowDialog();
            feladatok.First(x => x.IsSelected).IsSelected = false;
            RefreshUi();
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
