using BespokeFusion;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkApi.DTOs;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for FeladatokKezel.xaml
    /// </summary>
    public partial class FeladatokKezel : Window
    {
        private readonly ApiService _apiService;
        private int pageNumber = 0;
        private List<ErettsegizzunkApi.Models.Task> feladatok = new List<ErettsegizzunkApi.Models.Task>();
        public LoggedUser user;
        public FeladatokKezel(LoggedUser user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.user = user;
            RefreshUi();
        }

        private async Task<List<ErettsegizzunkApi.Models.Task>> LoadDatasAsync(int mettol)
        {
            feladatok.Clear();
            List<ErettsegizzunkApi.Models.Task> feladatoks = await _apiService.GetFeladatoksAsync(mettol);
            if(feladatoks is null)
            {
                MessageBoxes.CustomError("Hiba az adatok lekérdezése közben","Error");
                return new List<ErettsegizzunkApi.Models.Task>();
            }
            btnOldalKov.IsEnabled = feladatoks.Count == 50;//teszt
            return feladatoks;
        }

        private async void btnUjAdatokTxtbol_Click(object sender, RoutedEventArgs e)
        {
            string ret = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = "*txt",
                Filter = "txt|*.txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {                
                MessageBoxResult result = MessageBoxes.CustomQuestion($"Biztosan fel akarod tölteni a {openFileDialog.FileName.Split("\\").ToList().Last()} adatait?","Figyelem");
                if (result == MessageBoxResult.Cancel)
                {
                    MaterialMessageBox.Show("Feltöltés megszakítva!");
                    //MessageBox.Show("Feltöltés megszakítva!", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    ret = await _apiService.PostFeladatokFromTxt(feladatoks);
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (ret != string.Empty)
                    {
                        MessageBoxes.CustomMessage(ret);
                    }
                    RefreshUi();
                }
            }
        }

        private async void RefreshUi()
        {
            feladatok = await LoadDatasAsync(feladatok.Count == 50 ? feladatok[feladatok.Count - 1].Id : 0);//teszt
            dgFeladatAdatok.ItemsSource = feladatok;
            cbSelectAll.IsChecked = false;
        }

        private void btnOldalKov_Click(object sender, RoutedEventArgs e)
        {
            pageNumber++;
            RefreshUi();

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
            foreach (ErettsegizzunkApi.Models.Task feladat in feladatok)
            {
                feladat.IsSelected = true;
            }
            dgFeladatAdatok.Items.Refresh();
        }

        private void cbSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (ErettsegizzunkApi.Models.Task feladat in feladatok)
            {
                feladat.IsSelected = false;
            }
            dgFeladatAdatok.Items.Refresh();
        }

        private async void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (ErettsegizzunkApi.Models.Task feladat in feladatok)
            {
                if (feladat.IsSelected)
                {
                    ids.Add(feladat.Id);
                }
            }

            if (ids.Count < 1)
            {
                MessageBoxes.CustomMessage("Nincs törlendő elem kijelölve!");
                return;
            }

            MessageBoxes.CustomMessage(await _apiService.DeletFeladatok(new FeladatokDeleteDTO() { Ids = ids, Token = user.Token}));
            RefreshUi();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            Close();
        }
    }
}
