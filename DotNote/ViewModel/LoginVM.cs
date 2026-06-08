using DotNote.Model;
using DotNote.ViewModel.Commands.Login;
using DotNote.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace DotNote.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        public event EventHandler Authenticated;
        public event PropertyChangedEventHandler? PropertyChanged;


        #region Properties
        public bool IsShowingRegister { get; set; }

        private User user;

        public User User
        {
            get { return user; }
            set 
            { 
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set 
            { 
                email = value;
                UpdateUser();
                OnPropertyChanged(nameof(Email));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set 
            { 
                password = value;
                UpdateUser();
                OnPropertyChanged(nameof(Password));
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set 
            { 
                firstName = value;
                UpdateUser();
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set 
            { 
                lastName = value;
                UpdateUser();
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                UpdateUser();
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
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

        #region Helpers
        private void UpdateUser()
        {
            User = new User
            {
                Email = this.Email,
                Password = this.Password,
                FirstName = this.FirstName,
                LastName = this.LastName,
                ConfirmPassword = this.ConfirmPassword
            };
        }
        #endregion

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
            await App.DbHelper.Insert(User); // store user details in Db

            if (success) Authenticated?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
