using ErettsegizzunkAdmin.CustomMessageBoxes;
using ErettsegizzunkAdmin.Services;
using ErettsegizzunkApi.DTOs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for FeladatModosit.xaml
    /// </summary>
    public partial class FeladatModosit : Window
    {
        private ErettsegizzunkApi.Models.Task feladat;
        private readonly ApiService _apiService;
        private LoggedUserDTO user;
        public FeladatModosit(ErettsegizzunkApi.Models.Task feladat, LoggedUserDTO user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.feladat = feladat;
            DataContext = this.feladat;
            this.user = user;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string response = string.Empty;

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
                response = await _apiService.PutFeladatok(put);
                if (response is null)
                {
                    return;
                }
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(response);
                return;
            }
            MessageBoxes.CustomMessageOk(response);
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
