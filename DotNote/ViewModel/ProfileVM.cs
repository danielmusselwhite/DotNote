using DotNote.Model;
using DotNote.ViewModel.Commands;
using DotNote.ViewModel.Commands.Notes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DotNote.ViewModel
{
    public class ProfileVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
