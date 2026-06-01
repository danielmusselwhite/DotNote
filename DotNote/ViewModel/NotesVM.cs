using DotNote.Model;
using DotNote.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DotNote.ViewModel
{
    public class NotesVM
    {
        #region Properties
        public ObservableCollection<Notebook> Notebooks { get; set; }

        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set 
            { 
                selectedNotebook = value;
                // TODO - implement logic to load notes for selected notebook
            }
        }

        public ObservableCollection<Note> Notes { get; set; }
        #endregion

        #region Commands
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        #endregion

        public NotesVM()
        {
            Notebooks = new ObservableCollection<Notebook>();

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
        }
    }
}
