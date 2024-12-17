using BespokeFusion;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ErettsegizzunkAdmin.CustomMessageBoxes;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for FeladatokKezel.xaml
    /// </summary>
    public partial class FeladatokKezel : Window
    {
        private readonly ApiService _apiService;
        #warning ha egy feladat törlésre kerül bugos mert ugyanúgy a következő egész 100-tól kezd akkor is ha pl 101 már ott volt az előző oldalon
        private double pageNumber = 0.00;
        private List<Feladatok> feladatok = new List<Feladatok>();
        public FeladatokKezel()
        {
            InitializeComponent();
            _apiService = new ApiService();
            RefreshUi();
        }

        private async Task<List<Feladatok>> LoadDatasAsync()
        {
            List<Feladatok> feladatoks = await _apiService.GetFeladatoksAsync(pageNumber * 100);
            if(feladatoks is null)
            {
                MessageBoxes.CustomError("Hiba az adatok lekérdezése közben","Error");
                return new List<Feladatok>();
            }
            btnOldalKov.IsEnabled = feladatoks.Count == 100;
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
                    List<Feladatok> feladatoks = new List<Feladatok>();
                    StreamReader reader = new StreamReader(openFileDialog.FileName);
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        string[] sor = reader.ReadLine().Split("\t");
                        feladatoks.Add(new Feladatok { Leiras = sor[0], Megoldasok = sor[1], Helyese = sor[2], TantargyId = int.Parse(sor[3]), TipusId = int.Parse(sor[4]), SzintId = int.Parse(sor[5]) });
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
                        MessageBox.Show(ret);
                    }
                    RefreshUi();
                }
            }
        }

        private async void RefreshUi()
        {
            feladatok.Clear();
            feladatok = await LoadDatasAsync();
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

            if (feladatok.Count < 100)
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
            foreach (Feladatok feladat in feladatok)
            {
                feladat.Kijelolve = true;
            }
            dgFeladatAdatok.Items.Refresh();
        }

        private void cbSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Feladatok feladat in feladatok)
            {
                feladat.Kijelolve = false;
            }
            dgFeladatAdatok.Items.Refresh();
        }

        private async void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (Feladatok feladat in feladatok)
            {
                if (feladat.Kijelolve)
                {
                    ids.Add(feladat.Id);
                }
            }
            MessageBox.Show(await _apiService.DeletFeladatok(ids));
            RefreshUi();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
