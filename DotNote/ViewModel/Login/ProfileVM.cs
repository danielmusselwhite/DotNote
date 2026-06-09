using AutoMapper;
using DotNote.Model;
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
        #endregion

        #region events
        public event EventHandler ProfileUpdated;
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


        public ProfileVM(IDatabaseHelper db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            
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
            ProfileUpdated.Invoke(this, EventArgs.Empty);
        }

        internal void UpdatePassword()
        {
            throw new NotImplementedException();
        }
    }
}
