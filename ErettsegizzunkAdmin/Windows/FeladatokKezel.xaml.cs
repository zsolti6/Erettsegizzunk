using ErettsegizzunkAdmin.Services;
using System;
using System.Collections.Generic;
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
            var feladatok = await _apiService.GetFeladatoksAsync();
            if (feladatok != null)
            {
                dgFeladatAdatok.ItemsSource = feladatok;
            }
            else
            {
                MessageBox.Show("Failed to retrieve data.");
            }
        }
    }
}
