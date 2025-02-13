using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
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
    /// Interaction logic for UjTantargy.xaml
    /// </summary>
    public partial class UjTantargy : Window
    {
        private LoggedUserDTO user;
        private readonly ApiService _apiService;
        public UjTantargy(LoggedUserDTO user)
        {
            InitializeComponent();
            this.user = user;
            _apiService = new ApiService();
        }

        private async void btnTantargy_Letrehoz_Click(object sender, RoutedEventArgs e)
        {
            await _apiService.PostTantargy(new TantargyDTO() { Name = tbTantargyNev.Text, Token = user.Token });
            Close();
        }

        private void Megse_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
