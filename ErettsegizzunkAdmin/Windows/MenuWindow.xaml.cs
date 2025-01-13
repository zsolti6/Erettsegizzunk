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

namespace ErettsegizzunkAdmin.Windows
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        LoggedUser user;
        public MenuWindow(LoggedUser user)
        {
            InitializeComponent();
            this.user = user;
            lbUdvozles.Content = "Üdv: " + user.Name;
        }

        //Feladatok kezelése
        private void feladatKezelese_Click(object sender, RoutedEventArgs e)
        {
            FeladatokKezel kezel = new FeladatokKezel();
            kezel.ShowDialog();
        }

        //Feladatok témáinak kezelése
        private void feladatokTemaKezelese_Click(object sender, RoutedEventArgs e)
        {

        }

        //Minden egyéb kezelése
        private void segedKezelese_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
