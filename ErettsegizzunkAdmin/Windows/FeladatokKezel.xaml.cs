using ErettsegizzunkAdmin.Models;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for FeladatokKezel.xaml
    /// </summary>
    public partial class FeladatokKezel : Window
    {
        private readonly ApiService _apiService;
        public FeladatokKezel()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Setup();
        }

        private async void Setup()
        {
            List<Feladatok> feladatok = await _apiService.GetFeladatoksAsync();
            if (feladatok != null)
            {                
                dgFeladatAdatok.ItemsSource = feladatok;
            }
            else
            {
                MessageBox.Show("Failed to retrieve data.");
            }
            
        }

        private async void btnUjAdatokExcelbol_Click(object sender, RoutedEventArgs e)
        {
            string ret = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = "*csv",
                Filter = "csv|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
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
                }
                catch(FileNotFoundException ex)
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
    }
}
