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
using ErettsegizzunkApi.Models;
using ErettsegizzunkApi.DTOs;
using System.Drawing;
using System.IO;

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        LoggedUserDTO user;
        public MenuWindow(LoggedUserDTO user)
        {
            InitializeComponent();
            this.user = user;
            lbUdvozles.Content = "Üdv: " + user.Name;
            imgUser.Source = user.ProfilePicture;
        }

        //Feladatok kezelése
        private void feladatKezelese_Click(object sender, RoutedEventArgs e)
        {
            FeladatokKezel feladatok = new FeladatokKezel(user);
            feladatok.Show();
            Close();
        }

        //Feladatok témáinak kezelése
        private void feladatokTemaKezelese_Click(object sender, RoutedEventArgs e)
        {

        }

        //Minden egyéb kezelése
        private void segedKezelese_Click(object sender, RoutedEventArgs e)
        {

        }

        //Felhasználók kezelésére
        private void felhasznalokKezel_Click(object sender, RoutedEventArgs e)
        {
            FelhasznalokKezel felhasznalok = new FelhasznalokKezel(user);
            felhasznalok.Show();
            Close();
        }
    }
}
