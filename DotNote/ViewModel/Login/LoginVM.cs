using DotNote.Model;
using DotNote.ViewModel.Commands.Login;
using DotNote.ViewModel.Helpers;
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
        public event EventHandler Authenticated;

        #region Properties
        public bool IsShowingRegister { get; set; }
        #endregion

        #region Commands
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public SwitchShownLoginViewCommand SwitchShownLoginViewCommand { get; set; }
        #endregion

        public LoginVM()
        {
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
            var success = await FirebaseAuthHelper.Login(User);

            if (success) Authenticated?.Invoke(this, EventArgs.Empty);
        }

        public async void PerformRegister()
        {
            var success = await FirebaseAuthHelper.Register(User);

            if(success)
            {
                User.Id = App.UserId; // set the user id to the one returned by firebase
                await App.DbHelper.Insert(User); // store user details in Db
            }

            if (success) Authenticated?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
