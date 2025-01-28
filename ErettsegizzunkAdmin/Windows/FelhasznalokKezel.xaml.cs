using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
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
    /// Interaction logic for FelhasznalokKezel.xaml
    /// </summary>
    public partial class FelhasznalokKezel : Window
    {
        private LoggedUserDTO user;
        private List<User> felhasznalok = new List<User>();
        private List<FelhasznaloModotsitDTO> modositando = new List<FelhasznaloModotsitDTO>();
        private readonly ApiService _apiService;
        private int pageNumber = 0;

        public FelhasznalokKezel(LoggedUserDTO user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.user = user;
            RefreshUi();
        }

        private async Task<List<User>> LoadDatasAsync(int mettol)
        {
            felhasznalok.Clear();
            List<User> users = await _apiService.GetFelhasznalokAsync(new LoggedUserForCheckDTO() { Id = user.Id, Permission = user.Permission, Token = user.Token});
            if (users is null)
            {
                MessageBoxes.CustomError("Hiba az adatok lekérdezése közben", "Error");
                return new List<User>();
            }
            //btnOldalKov.IsEnabled = feladatoks.Count == 50;//teszt
            return users;
        }

        private async void RefreshUi()
        {
            felhasznalok = await LoadDatasAsync(pageNumber);
            dgAdatok.ItemsSource = felhasznalok;
        }

        private void btnUj_Click(object sender, RoutedEventArgs e)
        {
            UjFelhasznalo ujFelhasznalo = new UjFelhasznalo();
            ujFelhasznalo.ShowDialog();
            RefreshUi();

        }

        private async void btnTorol_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();

            foreach (User felhasznalo in felhasznalok)
            {
                if (felhasznalo.IsSelected)
                {
                    ids.Add(felhasznalo.Id);
                }
            }

            if (ids.Count < 1)
            {
                MessageBoxes.CustomMessage("Nincs törlendő elem kijelölve!");
                return;
            }

            MessageBoxes.CustomMessage(await _apiService.DeletFelhasznalok(new FelhasznaloTorolDTO() { Ids = ids, Token = user.Token }));
            RefreshUi();
        }

        private async void btnVissza_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow(user);
            menu.Show();
            if (modositando.Count != 0)
            {
                MessageBoxResult result = MessageBoxes.CustomQuestion("Menti a változtatásokat?");

                if (result == MessageBoxResult.OK)
                {
                    btnModosit_Click(sender, e);
                }
                else
                {
                    MessageBoxes.CustomMessageOk("Módosítások eldobva");
                }
            }
            Close();
        }

        private async void btnModosit_Click(object sender, RoutedEventArgs e)
        {
            await _apiService.PutFelhasznalok(new FelhasznaloModotsitDTO() { users = felhasznalok, Token = user.Token});
        }
    }
}
