using DotNote.Model;
using DotNote.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Profiles
{
    public class UpdateProfilePictureCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public ProfileVM VM { get; set; }

        public UpdateProfilePictureCommand(ProfileVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            ProfileVM profile = parameter as ProfileVM;
            return true;
        }

        public async void Execute(object? parameter)
        {
            await VM.UpdateProfilePicture();
        }
    }
}
