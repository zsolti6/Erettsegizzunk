using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for FeladatokKezel.xaml
    /// </summary>
    public partial class FeladatokKezel : Window
    {
        private readonly ApiService _apiService;
        private int pageNumber = 0;
        private List<Feladatok> feladatok = new List<Feladatok>();
        public FeladatokKezel()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Loaded += MyWindow_Loaded;
        }

        private async void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                feladatok = await LoadDatasAsync();
                dgFeladatAdatok.ItemsSource = feladatok;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization: {ex.Message}");
            }
        }

        private async Task<List<Feladatok>> LoadDatasAsync()
        {
            List<Feladatok> feladatoks = await _apiService.GetFeladatoksAsync(pageNumber * 100);
            if(feladatoks is null)
            {
                MessageBox.Show("Failed to retrieve data.");
                return null;
            }

            return feladatoks;
        }

        private async void btnUjAdatokExcelbol_Click(object sender, RoutedEventArgs e)
        {
            string ret = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = "*txt",
                Filter = "txt|*.txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                MessageBoxResult result = MessageBox.Show($"Biztosan fel akarod tölteni a {openFileDialog.FileName.Split("\\").ToList().Last()} adatait?","Figyelem",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    MessageBox.Show("Feltöltés megszakítva!", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    ret = await _apiService.PostFeladatokFromExcel(feladatoks);
                    //LoadDatas();
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
                }
            }
        }

        private async void btnOldalKov_Click(object sender, RoutedEventArgs e)
        {
            pageNumber++;

            feladatok.Clear();
            feladatok = await LoadDatasAsync();
            dgFeladatAdatok.ItemsSource = feladatok;

            if (pageNumber > 0)
            {
                btnOldalElozo.IsEnabled = true;
            }

            if (feladatok.Count < 100)
            {
                (sender as Button).IsEnabled = false;
            }
        }

        private async void btnOldalElozo_Click(object sender, RoutedEventArgs e)
        {
            pageNumber--;

            feladatok.Clear();
            feladatok = await LoadDatasAsync();
            dgFeladatAdatok.ItemsSource = feladatok;

            if (pageNumber < 1)
            {
                (sender as Button).IsEnabled = false;
            }

            if (btnOldalKov.IsEnabled == false)
            {
                btnOldalKov.IsEnabled = true;
            }
        }
    }
}
