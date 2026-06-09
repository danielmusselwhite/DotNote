using DotNote.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Login
{
    public class SwitchShownLoginViewCommand : ICommand
    {
        public LoginVM VM { get; set;  }

        public event EventHandler? CanExecuteChanged;

        public SwitchShownLoginViewCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.SwitchViews();
        }
    }
}
