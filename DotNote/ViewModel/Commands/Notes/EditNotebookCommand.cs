using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Notes
{
    public class EditNotebookCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public NotesVM VM { get; set; }

        public EditNotebookCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return parameter is NotebookVM;
        }

        public void Execute(object? parameter)
        {
            if (parameter is NotebookVM notebookVM)
            {
                VM.StartEditingNotebook(notebookVM);
            }
        }
    }
}
