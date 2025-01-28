using BespokeFusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;

namespace ErettsegizzunkAdmin.CustomMessageBoxes
{
    public static class MessageBoxes
    {

        public static void CustomError(string szoveg, string cim = "")
        {
            CustomMaterialMessageBox custom = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = szoveg, Foreground = Brushes.Black },
                TxtTitle = { Text = cim, Foreground = Brushes.Black },
                BtnOk = { Content = "Ok" },
                BtnCancel = { Visibility = Visibility.Collapsed },
                BtnCopyMessage = { Visibility = Visibility.Collapsed },
                MainContentControl = { Background = Brushes.WhiteSmoke },
                TitleBackgroundPanel = { Background = Brushes.Red },
                BorderBrush = Brushes.Red
            };
            custom.Show();
        }

        public static MessageBoxResult CustomQuestion(string szoveg, string cim="")
        {
            CustomMaterialMessageBox custom = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = szoveg, Foreground = Brushes.Black },
                TxtTitle = { Text = cim, Foreground = Brushes.Black },
                BtnOk = { Content = "Igen"},
                BtnCancel = { Content = "Nem" },
                BtnCopyMessage = { Visibility = Visibility.Collapsed },
                MainContentControl = { Background = Brushes.WhiteSmoke },
                TitleBackgroundPanel = { Background = (Brush)Application.Current.Resources["MaterialDesignDeepPurple"] },
                BorderBrush = Brushes.Purple
            };
            custom.ShowDialog();
            return custom.Result;
        }

        public static void CustomMessage(string szoveg, string cim = "")
        {
            CustomMaterialMessageBox custom = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = szoveg, Foreground = Brushes.Black },
                TxtTitle = { Text = cim, Foreground = Brushes.Black },
                BtnOk = { Content = "Igen"},
                BtnCancel = { Visibility = Visibility.Collapsed },
                BtnCopyMessage = { Visibility = Visibility.Collapsed },
                MainContentControl = { Background = Brushes.WhiteSmoke },
                TitleBackgroundPanel = { Background = Brushes.Purple },
                BorderBrush = Brushes.Purple
            };
            custom.Show();
        }

        public static void CustomMessageOk(string szoveg, string cim = "")
        {
            CustomMaterialMessageBox custom = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = szoveg, Foreground = Brushes.Black },
                TxtTitle = { Text = cim, Foreground = Brushes.Black },
                BtnOk = { Content = "Igen" },
                BtnCancel = { Visibility = Visibility.Collapsed },
                BtnCopyMessage = { Visibility = Visibility.Collapsed },
                MainContentControl = { Background = Brushes.WhiteSmoke },
                TitleBackgroundPanel = { Background = Brushes.Purple },
                BorderBrush = Brushes.Purple
            };
            custom.Show();
        }
    }
}
