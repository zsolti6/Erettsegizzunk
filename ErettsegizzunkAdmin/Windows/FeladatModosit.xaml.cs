using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.DTOs;
using ErettsegizzunkAdmin.Services;
using System.Windows;
using System.Windows.Controls;
using Task = ErettsegizzunkAdmin.Models.Task;

namespace ErettsegizzunkAdmin.Windows
{
    public partial class FeladatModosit : Window
    {
        private Task feladat;
        private readonly ApiService _apiService;
        private LoggedUserDTO user;
        private List<string> Themes;
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
                await GetCheckedTemak();
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
                    SzintId = feladat.LevelId,
                    Temak = Themes

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

        private async System.Threading.Tasks.Task GetCheckedTemak()
        {
            Themes = new List<string>();
            foreach (CheckBox tema in spTemak.Children)
            {
                if (tema.IsChecked == true)
                {
                    Themes.Add(tema.Content as string);
                }
            }
        }

        private async void InitializeAsync()
        {
            await SetLists();

            spTemak.Children.Clear();

            foreach (var item in feladat.ThemeList)
            {
                var checkBox = new CheckBox
                {
                    Content = item,
                    IsChecked = feladat.Themes.Select(x => x.Name).Contains(item),
                    Margin = new Thickness(5) // Optional: better spacing
                };

                if (Application.Current.TryFindResource("MaterialDesignCheckBox") is Style materialCheckBoxStyle)
                {
                    checkBox.Style = materialCheckBoxStyle;
                }

                spTemak.Children.Add(checkBox);
            }

            DataContext = feladat;
        }

    }
}
