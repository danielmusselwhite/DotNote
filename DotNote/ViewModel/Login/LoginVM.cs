using AutoMapper;
using DotNote.DTOs;
using DotNote.Model;
using DotNote.ViewModel.Commands.Login;
using DotNote.ViewModel.Helpers;
using DotNote.ViewModel.Helpers.DatabaseHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace DotNote.ViewModel.Login
{
    public class LoginVM : UserVM
    {
        #region dependencies
        private readonly FirebaseAuthHelper _authHelper;
        private readonly IDatabaseHelper _db;
        private readonly IMapper _mapper;
        #endregion

        // events
        public event EventHandler Authenticated;

        #region Properties
        public bool IsShowingRegister { get; set; }
        #endregion

        #region Commands
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public SwitchShownLoginViewCommand SwitchShownLoginViewCommand { get; set; }
        #endregion

        public LoginVM(FirebaseAuthHelper authHelper, IDatabaseHelper dbHelper, IMapper mapper)
        {
            _authHelper = authHelper;
            _db = dbHelper;
            _mapper = mapper;

            User = new User();

            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
            SwitchShownLoginViewCommand = new SwitchShownLoginViewCommand(this);
        }

        #region Command Handlers
        public void SwitchViews()
        {
            IsShowingRegister = !IsShowingRegister;

            OnPropertyChanged(nameof(IsShowingRegister));
        }

        public async void PerformLogin()
        {
            var dto = _mapper.Map<FirebaseAuthDTO>(User);
            var success = await _authHelper.Login(dto);

            if (success)
                Authenticated?.Invoke(this, EventArgs.Empty);
        }

        public async void PerformRegister()
        {
            var dto = _mapper.Map<FirebaseAuthDTO>(User);
            var success = await _authHelper.Register(dto);

            if(success)
            {
                var profile = _mapper.Map<UserDetails>(User);

                profile.UserId = App.UserId; // set the user id to the one returned by firebase
                await _db.Insert(profile); // store user details in Db
            }

            if (success) Authenticated?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
