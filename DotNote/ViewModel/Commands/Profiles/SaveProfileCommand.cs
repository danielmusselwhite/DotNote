using DotNote.Model;
using DotNote.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Profiles
{
    public class SaveProfileCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public ProfileVM VM { get; set; }

        public SaveProfileCommand(ProfileVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true; // todo - modify this so can only be saved if name has value
        }

        public async void Execute(object? parameter)
        {
            await VM.SaveProfile();
        }
    }
}
