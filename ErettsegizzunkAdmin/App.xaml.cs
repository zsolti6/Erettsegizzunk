using DotNetEnv;
using ErettsegizzunkAdmin.Services;
using System.Windows;

namespace ErettsegizzunkAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int id = -1;
        public static string token = string.Empty;
        public static string ftpUrl = "";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Hook ProcessExit for abrupt process termination
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            // Hook UnhandledException for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // You could also include Application.DispatcherUnhandledException for UI-thread exceptions:
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            Env.Load();

            ftpUrl = Env.GetString("FTP_URL");

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Ensure that your `OnApplicationExit` logic runs here.
            OnApplicationExit(this, e);
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            OnApplicationExit(sender, null);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Optional: Log or handle the exception here if needed
            Exception exception = e.ExceptionObject as Exception;
            Console.WriteLine($"Unhandled Exception: {exception?.Message}");

            // Perform cleanup tasks
            OnApplicationExit(sender, null);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Handle UI thread exceptions here
            Console.WriteLine($"UI Thread Exception: {e.Exception.Message}");
            e.Handled = true;

            // Perform cleanup tasks
            OnApplicationExit(sender, null);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (id == -1)
            {
                return;
            }

            ApiService _apiService = new ApiService();

            // Since this is an async call, you should handle it appropriately
            try
            {
                // Prefer async void sparingly; use fire-and-forget here
                _ = _apiService.LogOut(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log out: {ex.Message}");
            }
        }
    }


}
