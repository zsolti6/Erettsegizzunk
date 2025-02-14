using BespokeFusion;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Media;

namespace ErettsegizzunkAdmin.CustomMessageBoxes
{
    public static class MessageBoxes
    {
        public static void CustomError(string szoveg, string cim = "Hiba")
        {
            CustomMaterialMessageBox custom = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = szoveg, Foreground = Brushes.Black, FontSize = 15 },
                TxtTitle = { Text = cim, Foreground = Brushes.White },
                BtnOk = { Content = "Ok", Background = Brushes.Red },
                BtnCancel = { Visibility = Visibility.Collapsed },
                MainContentControl = { Background = Brushes.WhiteSmoke },
                TitleBackgroundPanel = { Background = Brushes.Red },
                BorderBrush = Brushes.Red,
                CustomIcon = PackIconKind.Warning,
                CustomIconForeground = Brushes.Red
            };

            custom.Show();
        }

        public static MessageBoxResult CustomQuestion(string szoveg, string cim = "Figyelem")
        {
            Color color = (Color)ColorConverter.ConvertFromString("#440FB5"); //-----> PrimaryDark-hoz illik
            SolidColorBrush brush = new SolidColorBrush(color);

            CustomMaterialMessageBox custom = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = szoveg, Foreground = Brushes.Black, FontSize = 15 },
                TxtTitle = { Text = cim, Foreground = Brushes.White },
                BtnOk = { Content = "Igen", Background = brush },
                BtnCancel = { Content = "Nem", Background = Brushes.DodgerBlue },
                BtnCopyMessage = { Visibility = Visibility.Collapsed },
                MainContentControl = { Background = Brushes.WhiteSmoke },
                TitleBackgroundPanel = { Background = brush },
                BorderBrush = brush,
                CustomIcon = PackIconKind.QuestionMarkCircle,
                CustomIconForeground = brush
            };
            custom.ShowDialog();
            return custom.Result;
        }

        public static void CustomMessageOk(string szoveg, string cim = "Üzenet")
        {
            Color color = (Color)ColorConverter.ConvertFromString("#320b86"); //-----> PrimaryDark 
            SolidColorBrush brush = new SolidColorBrush(color);

            CustomMaterialMessageBox custom = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = szoveg, Foreground = Brushes.Black, FontSize = 15 },
                TxtTitle = { Text = cim, Foreground = Brushes.White },
                BtnOk = { Content = "Ok", Background = brush },
                BtnCancel = { Visibility = Visibility.Collapsed },
                BtnCopyMessage = { Visibility = Visibility.Collapsed },
                MainContentControl = { Background = Brushes.WhiteSmoke },
                TitleBackgroundPanel = { Background = brush },
                BorderBrush = brush,
                CustomIcon = PackIconKind.InformationCircle,
                CustomIconForeground = brush
            };
            custom.Show();
        }
    }
}
