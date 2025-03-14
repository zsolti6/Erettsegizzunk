using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using Task = ErettsegizzunkApi.Models.Task;

namespace ErettsegizzunkAdmin.Windows
{
    public partial class FeladatModosit : Window
    {
        private Task feladat;
        private readonly ApiService _apiService;
        private LoggedUserDTO user;
        public FeladatModosit(Task feladat, LoggedUserDTO user)
        {
            _apiService = new ApiService();
            this.user = user;
            this.feladat = feladat;
            InitializeComponent();
            InitializeAsync();
        }

        private async System.Threading.Tasks.Task SetLists()
        {
            feladat.SubjectList = (await _apiService.GetTantargyaksAsync()).Select(x => x.Name).ToList();
            feladat.LevelList = (await _apiService.GetLevelAsync()).Select(x => x.Name).ToList();
            feladat.TypeList = (await _apiService.GetTipusAsync()).Select(x => x.Name).ToList();
            feladat.ThemeList = (await _apiService.GetTemakAsync()).Select(x => x.Name).ToList();
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FeladatokPutPostDTO put = new FeladatokPutPostDTO()
                {
                    Id = feladat.Id,
                    Token = user.Token,
                    KepNev = feladat.PicName,
                    Leiras = feladat.Description,
                    Szoveg = feladat.Text,
                    Megoldasok = feladat.Answers,
                    Helyese = feladat.IsCorrect,
                    TantargyId = feladat.SubjectId,
                    TipusId = feladat.TypeId,
                    SzintId = feladat.LevelId

                };
                await _apiService.PutFeladatok(put);
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(new ErrorDTO(516, "Hiba történt az adatok mentése közben").ToString());
                return;
            }
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void InitializeAsync()
        {
            await SetLists();

            // Clear any existing children
            spTemak.Children.Clear();

            foreach (var item in feladat.ThemeList)
            {
                // Create the CheckBox
                var checkBox = new CheckBox
                {
                    Content = item,
                    IsChecked = feladat.Themes.Select(x => x.Name).Contains(item)
                };

                // Apply the Material Design style for CheckBox, if available.
                // The key "MaterialDesignCheckBox" is used in MaterialDesignInXAML.
                if (Application.Current.TryFindResource("MaterialDesignCheckBox") is Style materialCheckBoxStyle)
                {
                    checkBox.Style = materialCheckBoxStyle;
                }

                // Add the CheckBox to the StackPanel
                spTemak.Children.Add(checkBox);
            }

            DataContext = feladat;
        }
    }
}
