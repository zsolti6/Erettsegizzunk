using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Models;
using ErettsegizzunkAdmin.Services;
using System.Windows;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for UjPermission.xaml
    /// </summary>
    public partial class UjPermission : Window
    {
        private LoggedUserDTO user;
        private readonly ApiService _apiService;
        private int[] Kizarva;
        public UjPermission(LoggedUserDTO user)
        {
            InitializeComponent();
            this.user = user;
            _apiService = new ApiService();
        }

        private async void btnTantargy_Letrehoz_Click(object sender, RoutedEventArgs e)
        {
            await _apiService.PostPermission(new PostPermissionDTO() { Permission = new Permission { Name = tbPermissionName.Text, Description = tbPermissionDescription.Text, Level = int.Parse(tbPermissionLevel.Text.ToString()) }, Token = user.Token });
            Close();
        }

        private void Megse_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
