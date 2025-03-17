using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Models;
using ErettsegizzunkAdmin.Services;
using System.Windows;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for UjTema.xaml
    /// </summary>
    public partial class UjTema : Window
    {
        private LoggedUserDTO user;
        private readonly ApiService _apiService;
        public UjTema(LoggedUserDTO user)
        {
            InitializeComponent();
            this.user = user;
            _apiService = new ApiService();
        }

        private async void btnTantargy_Letrehoz_Click(object sender, RoutedEventArgs e)
        {
            await _apiService.PostTema(new PostThemeDTO() { Theme = new Theme { Name = tbTemaNev.Text }, Token = user.Token });
            Close();
        }

        private void Megse_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
