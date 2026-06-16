using DotNote.Configuration;
using DotNote.Model;
using DotNote.ViewModel.Helpers.StorageHelpers;
using DotNote.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotNote.View
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private readonly ProfileVM VM;
        private readonly AzureBlobHelper _azureBlobHelper;

        public ProfileWindow(ProfileVM vm, AzureBlobHelper azureBlobHelper, UserDetails user)
        {
            InitializeComponent();

            // todo - should UserDetails be injected in the ProfileVM instead of ProfileWindow so this logic can be placed in the ProfileVM's constructor instead?
            VM = vm;
            _azureBlobHelper = azureBlobHelper;
            DataContext = VM;

            VM.Email = user.Email;
            VM.FirstName = user.FirstName;
            VM.LastName = user.LastName;
            VM.UserId = user.UserId;
            VM.ProfileId = user.Id;

            // check if user has profilepicturepath (azure storage blob) and if so, load it into the ProfilePictureImage control
            VM.ProfilePictureBlobName = user.ProfilePictureBlobName;

            VM.ProfileUpdateFinished += VM_ProfileUpdateFinished;
            Loaded += ProfileWindow_Loaded;
        }

        private async void ProfileWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(VM.ProfilePictureBlobName))
            {
                try
                {
                    // Load the profile picture from Azure Storage Blob
                    var blob = await _azureBlobHelper.GetStreamFromBlobAsync(VM.ProfilePictureBlobName, AppSettings.AzureStorage.UserPhotosContainerName);
                    blob.Position = 0;
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = blob;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    VM.ProfilePicture = bitmap;
                }
                catch (Exception ex)
                {
                    // todo - add default profile picture if the image fails to load
                }
            }
            else
            {
                // todo - add default profile picture if the user doesn't have one
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VM_ProfileUpdateFinished(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
