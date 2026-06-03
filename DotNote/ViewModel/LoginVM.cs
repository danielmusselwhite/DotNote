using DotNote.Model;
using DotNote.ViewModel.Commands.Login;
using DotNote.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace DotNote.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        public event EventHandler Authenticated;
        public event PropertyChangedEventHandler? PropertyChanged;


        #region Properties
        private bool isShowingRegister = false;

        private Visibility loginVis;
        public Visibility LoginVis
        {
            get { return loginVis; }
            set 
            { 
                loginVis = value;
                OnPropertyChanged(nameof(LoginVis));
            }
        }

        private Visibility registerVis;
        public Visibility RegisterVis
        {
            get { return registerVis; }
            set 
            { 
                registerVis = value;
                OnPropertyChanged(nameof(RegisterVis));
            }
        }

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

            LoginVis = Visibility.Visible;
            RegisterVis = Visibility.Collapsed;

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
            User = new User(); // clear the user details

            isShowingRegister = !isShowingRegister;

            if (isShowingRegister)
            {
                RegisterVis = Visibility.Visible;
                LoginVis = Visibility.Collapsed;
            }
            else
            {
                RegisterVis = Visibility.Collapsed;
                LoginVis = Visibility.Visible;
            }
        }

        public async void PerformLogin()
        {
            var success = await FirebaseAuthHelper.Login(User);

            if (success) Authenticated?.Invoke(this, EventArgs.Empty);
        }

        public async void PerformRegister()
        {
            var success = await FirebaseAuthHelper.Register(User);

            if(success) Authenticated?.Invoke(this, EventArgs.Empty);
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
