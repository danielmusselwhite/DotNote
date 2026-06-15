using DotNote.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DotNote.ViewModel.Commands.Notes
{
    public class DeleteNotebookCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public NotesVM VM { get; set; }

        public DeleteNotebookCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            NotebookVM notebookVM = parameter as NotebookVM;
            if (notebookVM == null) return;
            VM.DeleteNotebook(notebookVM);
        }
    }
}
