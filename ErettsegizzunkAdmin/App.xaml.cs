using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using System.Windows;

namespace ErettsegizzunkAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int id = 0;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Exit += OnApplicationExit;
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            ApiService _apiService = new ApiService();
            Task<string> answ = _apiService.LogOut(new ModifyToken() {Id = id, Aktiv = false, LogOut = DateTime.Now });
        }
    }


}
