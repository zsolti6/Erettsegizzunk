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
            }
            catch (Exception)
            {
                MessageBoxes.CustomError(response);
                return;
            }
            MessageBoxes.CustomMessageOk(response);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            // Create and configure OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Válasszon ki egy képet"
            };

            // Show dialog and load image if a file is selected
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Load the selected image
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    // Set the image source for preview
                    ImagePreview.Source = bitmap;

                    string base64Image = ConvertImageToBase64(openFileDialog.FileName);

                    // Optionally send the Base64 string to your backend.
                    await _apiService.UploadImageToBackendAsync(base64Image);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string ConvertImageToBase64(string filePath)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageBytes);
        }

    }
}
