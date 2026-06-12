using AutoMapper;
using DotNote.Model;
using DotNote.View.UserControls;
using DotNote.ViewModel.Commands;
using DotNote.ViewModel.Commands.Notes;
using DotNote.ViewModel.Commands.Profiles;
using DotNote.ViewModel.Helpers;
using DotNote.ViewModel.Helpers.DatabaseHelpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DotNote.ViewModel.Login
{
    public class ProfileVM : UserVM
    {
        #region dependencies
        private readonly IDatabaseHelper _db;
        private readonly IMapper _mapper;
        private readonly FirebaseAuthHelper _auth;
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

        #endregion

        #region commands
        public SaveProfileCommand SaveProfileCommand { get; set; }
        public UpdatePasswordCommand UpdatePasswordCommand { get; set; }
        #endregion


        public ProfileVM(IDatabaseHelper db, IMapper mapper, FirebaseAuthHelper auth)
        {
            _db = db;
            _mapper = mapper;
            _auth = auth;

            SaveProfileCommand = new SaveProfileCommand(this);
            UpdatePasswordCommand = new UpdatePasswordCommand(this);
        }

        internal async Task SaveProfile()
        {
            var user = this.User;
            var profile = _mapper.Map<UserDetails>(user);
            profile.UserId = UserId;
            profile.Id = ProfileId;
            await _db.Update(_mapper.Map<UserDetails>(profile));
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
