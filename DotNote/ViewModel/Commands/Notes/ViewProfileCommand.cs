using DotNote.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Notes
{
    public class ViewProfileCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public NotesVM VM { get; set; }

        public ViewProfileCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            await VM.ViewProfile();
        }
    }
}
