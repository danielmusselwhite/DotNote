using DotNote.Model;
using DotNote.ViewModel.Commands;
using DotNote.ViewModel.Commands.Notes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DotNote.ViewModel.Login
{
    public abstract class UserVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        #region Properties
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

        #region Event Handlers
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
