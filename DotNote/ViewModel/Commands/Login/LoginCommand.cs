using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginVM VM { get; set; }

        public event EventHandler? CanExecuteChanged;

        public LoginCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true; // TODO - implement logic to determine if command can execute (e.g. validate input fields)
        }

        public void Execute(object? parameter)
        {
            // TODO - implement login logic (e.g. validate user credentials, navigate to main page, etc.)
        }
    }
}
