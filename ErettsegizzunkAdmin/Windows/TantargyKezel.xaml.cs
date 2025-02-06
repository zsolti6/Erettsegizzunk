using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for TantargyKezel.xaml
    /// </summary>
    public partial class TantargyKezel : Window
    {
        #region Bezaras gomb eltüntetése
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        const uint MF_GRAYED = 0x00000001;
        const uint MF_ENABLED = 0x00000000;
        const uint SC_CLOSE = 0xF060;
        #endregion

        private LoggedUserDTO user = new LoggedUserDTO();
        private List<Subject> subjects = new List<Subject>();
        private readonly ApiService _apiService;
        public TantargyKezel(LoggedUserDTO user)
        {
            InitializeComponent();
            this.user = user;
            _apiService = new ApiService();
            RefreshUi();
        }

        private async void RefreshUi()
        {
            subjects = await _apiService.GetTantargyaksAsync();
            dgTantargyAdatok.ItemsSource = subjects;
            cbSelectAll.IsChecked = false;
        }

        private void cbSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Subject subject in subjects)
            {
                subject.IsSelected = true;
            }
            dgTantargyAdatok.Items.Refresh();
        }

        private void cbSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Subject subject in subjects)
            {
                subject.IsSelected = false;
            }
            dgTantargyAdatok.Items.Refresh();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            Close();
        }

        private async void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (Subject subject in subjects)
            {
                if (subject.IsSelected)
                {
                    ids.Add(subject.Id);
                }
            }

            if (ids.Count < 1)
            {
                MessageBoxes.CustomMessageOk("Nincs törlendő elem kijelölve!");
                return;
            }

            MessageBoxes.CustomMessageOk(await _apiService.DeletTantargy(new TantargyDeleteDTO() { Ids = ids, Token = user.Token }));
            RefreshUi();
        }

        private void btnUjTantagyFelvitele_Click(object sender, RoutedEventArgs e)
        {
            UjTantargy ujTantargy = new UjTantargy(user);
            ujTantargy.ShowDialog();
        }

        private void btnQuestion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnModosit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hMenu = GetSystemMenu(hwnd, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }
    }
}
