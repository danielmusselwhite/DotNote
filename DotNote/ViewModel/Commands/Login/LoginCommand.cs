using DotNote.Model;
using DotNote.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Login
{
    public class LoginCommand : ICommand
    {
        public LoginVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public LoginCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            User user = parameter as User;

            if (user == null
                || string.IsNullOrWhiteSpace(user.Email)
                || string.IsNullOrWhiteSpace(user.Password)) 
                return false;


            return true;
        }

        public void Execute(object? parameter)
        {
            VM.PerformLogin();
        }
    }
}
