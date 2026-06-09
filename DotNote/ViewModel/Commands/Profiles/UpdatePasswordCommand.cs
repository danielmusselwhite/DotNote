using DotNote.Model;
using DotNote.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Profiles
{
    public class UpdatePasswordCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public ProfileVM VM { get; set; }

        public UpdatePasswordCommand(ProfileVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.UpdatePassword();
        }
    }
}
