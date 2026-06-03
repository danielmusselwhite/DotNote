using DotNote.Model;
using DotNote.ViewModel.Commands;
using DotNote.ViewModel.Commands.Notes;
using DotNote.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace DotNote.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
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
                OnPropertyChanged(nameof(SelectedNotebook));

                GetNotes();
            }
        }

        public ObservableCollection<Note> Notes { get; set; }
        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set 
            { 
                selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                SelectedNoteChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        private Visibility isNotebookEditVisible;
        public Visibility IsNotebookEditVisible
        {
            get { return isNotebookEditVisible; }
            set
            {
                isNotebookEditVisible = value;
                OnPropertyChanged(nameof(IsNotebookEditVisible));
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler SelectedNoteChanged;
        #endregion

        #region Commands
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditNotebookCommand EditNotebookCommand { get; set; }
        public EndEditNotebookCommand EndEditNotebookCommand { get; set; }
        #endregion

        #region Constructor
        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            EditNotebookCommand = new EditNotebookCommand(this);
            EndEditNotebookCommand = new EndEditNotebookCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            IsNotebookEditVisible = Visibility.Collapsed;
        }
        #endregion

        #region Methods
        #region Notebook Methods
        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook
            {
                Name = "New Notebook",
                UserId = App.UserId
            };

            DatabaseHelper.Insert(newNotebook);

            GetNotebooks();
        }

        public void GetNotebooks()
        {
            var notebooks = DatabaseHelper.GetAll<Notebook>()
                .Where(n => n.UserId == App.UserId)
                .ToList();

            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        public void StartEditingNotebook()
        {
            IsNotebookEditVisible = Visibility.Visible;
        }

        public void StopEditingNotebook(Notebook notebook)
        {
            IsNotebookEditVisible = Visibility.Collapsed;
            DatabaseHelper.Update(notebook);
            GetNotebooks();
        }
        #endregion

        #region Note Methods
        public void CreateNote(int notebookId)
        {
            Note newNote = new Note
            {
                NotebookId = notebookId,
                Title = $"New Note",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            DatabaseHelper.Insert(newNote);

            GetNotes();
        }

        private void GetNotes()
        {
            if (SelectedNotebook == null || SelectedNotebook.Id == 0) return;

            var notes = DatabaseHelper.GetAll<Note>()
                .Where(n => n.NotebookId == SelectedNotebook.Id);

            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }
        #endregion
        #endregion

        #region Helpers
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
