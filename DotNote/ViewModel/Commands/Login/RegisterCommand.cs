using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Login
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }

        public event EventHandler? CanExecuteChanged;

        public RegisterCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true; // TODO - implement logic to determine if command can execute (e.g. validate input fields)
        }

        public void Execute(object? parameter)
        {
            // TODO - implement registration logic (e.g. create new user in database, navigate to login page, etc.)
        }
    }
}
