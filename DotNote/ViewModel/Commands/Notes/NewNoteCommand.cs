using DotNote.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public NewNoteCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            var selectedNotebook = parameter as Notebook;
            if (selectedNotebook == null) return false;
            return true;
        }

        public async void Execute(object? parameter)
        {
            var selectedNotebook = parameter as Notebook;

            if(selectedNotebook == null) return;

            await VM.CreateNote(selectedNotebook.Id);
        }
    }
}
