using AutoMapper;
using DotNote.Configuration;
using DotNote.Model;
using DotNote.View.UserControls;
using DotNote.ViewModel.Commands;
using DotNote.ViewModel.Commands.Notes;
using DotNote.ViewModel.Commands.Profiles;
using DotNote.ViewModel.Helpers;
using DotNote.ViewModel.Helpers.DatabaseHelpers;
using DotNote.ViewModel.Helpers.StorageHelpers;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DotNote.ViewModel.Login
{
    public class ProfileVM : UserVM
    {
        #region dependencies
        private readonly IDatabaseHelper _db;
        private readonly IMapper _mapper;
        private readonly FirebaseAuthHelper _auth;
        private readonly AzureBlobHelper _azureBlobHelper;
        #endregion

        #region events
        public event EventHandler ProfileUpdateFinished;
        #endregion

        #region properties
        private string userId;
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private string profileId;
        public string ProfileId
        {
            get { return profileId; }
            set { profileId = value; }
        }

        private bool profilePictureChanged;

        private ImageSource _profilePicture;
        public ImageSource ProfilePicture
        {
            get => _profilePicture;
            set
            {
                _profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }

        private string _profilePictureLocalPath;
        
        public string ProfilePictureBlobName { get; set; }
        #endregion

        #region commands
        public SaveProfileCommand SaveProfileCommand { get; set; }
        public UpdatePasswordCommand UpdatePasswordCommand { get; set; }
        public UpdateProfilePictureCommand UpdateProfilePictureCommand { get; set; }
        #endregion


        public ProfileVM(IDatabaseHelper db, IMapper mapper, FirebaseAuthHelper auth, AzureBlobHelper azureBlobHelper)
        {
            _db = db;
            _mapper = mapper;
            _auth = auth;
            _azureBlobHelper = azureBlobHelper;

            SaveProfileCommand = new SaveProfileCommand(this);
            UpdatePasswordCommand = new UpdatePasswordCommand(this);
            UpdateProfilePictureCommand = new UpdateProfilePictureCommand(this);
        }

        public async Task UpdateProfilePicture()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() != true)
                return;

            _profilePictureLocalPath = ResizeImage(dialog.FileName);

            ProfilePicture = new BitmapImage(
                new Uri(_profilePictureLocalPath));

            profilePictureChanged = true;
        }

        private string ResizeImage(string sourcePath)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(sourcePath);
            bitmap.DecodePixelWidth = 250;
            bitmap.DecodePixelHeight = 250;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            var encoder = new JpegBitmapEncoder
            {
                QualityLevel = 85
            };

            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            var tempFile = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.jpg");

            using var stream = File.Create(tempFile);
            encoder.Save(stream);

            return tempFile;
        }

        internal async Task SaveProfile()
        {

            var user = this.User;
            var profile = _mapper.Map<UserDetails>(user);

            // adding additional fields not existant in UserVM
            profile.UserId = UserId;
            profile.Id = ProfileId;
            profile.ProfilePictureBlobName = ProfilePictureBlobName;

            // uploading profile picture to Azure Blob Storage and updating the profile picture path in the database
            if (profilePictureChanged)
            {
                var fileExtension = System.IO.Path.GetExtension(_profilePictureLocalPath);
                profile.ProfilePictureBlobName = $"{UserId}{fileExtension}";
                await _azureBlobHelper.UploadBlobAsync(_profilePictureLocalPath, profile.ProfilePictureBlobName, AppSettings.AzureStorage.UserPhotosContainerName);
            }

            await _db.Update(profile);
            ProfileUpdateFinished.Invoke(this, EventArgs.Empty);
        }

        internal async void UpdatePassword()
        {
            var success = await _auth.SendPasswordReset(App.LoggedInUser!.email);
            if (!success) MessageBox.Show("Failed To Send Password Reset Email, Please Try Again Later");
            else MessageBox.Show("Password Reset Email Sent");
        }
    }
}
