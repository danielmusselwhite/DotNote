using DotNote.Model;
using DotNote.ViewModel.Commands;
using DotNote.ViewModel.Commands.Notes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

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

                _ = GetNotes(); // is this okay as it is async?
            }
        }

        ObservableCollection<FontFamily> FontFamilies;

        public ObservableCollection<Note> Notes { get; set; }
        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set 
            { 
                selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                OnPropertyChanged(nameof(IsNoteSelected));
                SelectedNoteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsNoteSelected
        {
            get { return SelectedNote != null; }
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
        public DeleteNotebookCommand DeleteNotebookCommand { get; set; }
        public EditNotebookCommand EditNotebookCommand { get; set; }
        public EndEditNotebookCommand EndEditNotebookCommand { get; set; }
        public ViewProfileCommand ViewProfileCommand { get; set; }
        #endregion

        #region Constructor
        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            DeleteNotebookCommand = new DeleteNotebookCommand(this);
            EditNotebookCommand = new EditNotebookCommand(this);
            EndEditNotebookCommand = new EndEditNotebookCommand(this);
            ViewProfileCommand = new ViewProfileCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            FontFamilies = new ObservableCollection<FontFamily>(Fonts.SystemFontFamilies.OrderBy(f => f.Source));

            IsNotebookEditVisible = Visibility.Collapsed;
        }
        #endregion

        #region Methods
        #region Notebook Methods
        public async void CreateNotebook()
        {
            Notebook newNotebook = new Notebook
            {
                Name = "New Notebook",
                UserId = App.UserId
            };

            await App.DbHelper.Insert(newNotebook);

            GetNotebooks();
        }

        public async void GetNotebooks()
        {
            var notebooks = (await App.DbHelper.GetAll<Notebook>())
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

        public async void StopEditingNotebook(Notebook notebook)
        {
            IsNotebookEditVisible = Visibility.Collapsed;
            await App.DbHelper.Update(notebook);
            GetNotebooks();
        }

        public async void DeleteNotebook(Notebook notebook)
        {
            if (notebook == null) return;
            await App.DbHelper.Delete(notebook);
            GetNotebooks();
        }
        #endregion

        #region Note Methods
        public async void CreateNote(string notebookId)
        {
            Note newNote = new Note
            {
                NotebookId = notebookId,
                Title = $"New Note",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await App.DbHelper.Insert(newNote);

            await GetNotes();
        }

        public async Task GetNotes()
        {
            if (SelectedNotebook == null || string.IsNullOrWhiteSpace(SelectedNotebook.Id)) return;

            var notes = (await App.DbHelper.GetAll<Note>())
                .Where(n => n.NotebookId == SelectedNotebook.Id);

            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }
        #endregion

        public async void ViewProfile()
        {
            var user = await App.DbHelper.GetById<User>(App.UserId);
            var profileWindow = new View.ProfileWindow(user);
            profileWindow.ShowDialog();
        }

        #endregion

            #region Helpers
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
