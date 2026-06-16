using AutoMapper;
using DotNote.Configuration;
using DotNote.Model;
using DotNote.View;
using DotNote.ViewModel.Commands;
using DotNote.ViewModel.Commands.Notes;
using DotNote.ViewModel.Helpers;
using DotNote.ViewModel.Helpers.DatabaseHelpers;
using DotNote.ViewModel.Helpers.StorageHelpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DotNote.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        #region dependencies
        private readonly IMapper _mapper;
        private readonly IDatabaseHelper _db;
        private readonly AzureBlobHelper _azureBlobHelper;
        private readonly Func<UserDetails, ProfileWindow> _profileWindowFactory;
        #endregion

        #region Properties
        public ObservableCollection<NotebookVM> NotebookVMs { get; set; }
        private NotebookVM selectedNotebookVM;
        public NotebookVM SelectedNotebookVM
        {
            get { return selectedNotebookVM; }
            set 
            { 
                selectedNotebookVM = value;
                OnPropertyChanged(nameof(SelectedNotebookVM));

                _ = GetNotes(); // is this okay as it is async?
            }
        }

        public ObservableCollection<FontFamily> FontFamilies { get; set; }

        public ObservableCollection<int> FontSizes { get; set; }

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

        private NotebookVM _editingNotebook; // Keep track of the notebook being edited

        private ImageSource profilePicture;
        public ImageSource ProfilePicture
        {
            get { return profilePicture; }
            set
            {
                profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
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
        public NotesVM(
            IMapper mapper,
            IDatabaseHelper databaseHelper,
            AzureBlobHelper azureBlobHelper,
            Func<UserDetails, ProfileWindow> profileWindowFactory)
        {
            _mapper = mapper;
            _db = databaseHelper;
            _azureBlobHelper = azureBlobHelper;
            _profileWindowFactory = profileWindowFactory;

            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            DeleteNotebookCommand = new DeleteNotebookCommand(this);
            EditNotebookCommand = new EditNotebookCommand(this);
            EndEditNotebookCommand = new EndEditNotebookCommand(this);
            ViewProfileCommand = new ViewProfileCommand(this);

            NotebookVMs = new ObservableCollection<NotebookVM>();
            Notes = new ObservableCollection<Note>();

            FontFamilies = new ObservableCollection<FontFamily>(Fonts.SystemFontFamilies.OrderBy(f => f.Source));
            FontSizes = new ObservableCollection<int>(new int[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
        }
        #endregion

        #region Methods
        #region Notebook Methods
        public async Task CreateNotebook()
        {
            Notebook newNotebook = new Notebook
            {
                Name = "New Notebook",
                UserId = App.LoggedInUser!.localId
            };

            await _db.Insert(newNotebook);

            await GetNotebooks();
        }

        public async Task GetNotebooks()
        {
            var notebooks = (await _db.GetAll<Notebook>())
                .Where(n => n.UserId == App.LoggedInUser!.localId)
                .ToList();

            NotebookVMs.Clear();
            foreach (var notebook in notebooks)
            {
                NotebookVMs.Add(new NotebookVM(notebook));
            }
        }

        public void StartEditingNotebook(NotebookVM notebookVM)
        {
            // If another notebook is currently being edited, stop editing it
            if (_editingNotebook != null)
                _editingNotebook.IsRenaming = false;

            // Set the new notebook as the one being edited
            _editingNotebook = notebookVM;
            notebookVM.IsRenaming = true;
        }

        public async Task StopEditingNotebook(NotebookVM notebookVM)
        {
            // Update the DB
            await _db.Update(notebookVM.Model);

            // Reset the editing state
            _editingNotebook?.IsRenaming = false;
            _editingNotebook = null;
        }

        public async Task DeleteNotebook(NotebookVM notebookVM)
        {
            if (notebookVM == null) return;
            await _db.Delete(notebookVM.Model);
            await GetNotebooks();
        }
        #endregion

        #region Note Methods
        public async Task CreateNote(string notebookId)
        {
            Note newNote = new Note
            {
                NotebookId = notebookId,
                Title = $"New Note",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _db.Insert(newNote);

            await GetNotes();
        }

        public async Task GetNotes()
        {
            if (SelectedNotebookVM == null || string.IsNullOrWhiteSpace(SelectedNotebookVM.Model.Id)) return;

            var notes = (await _db.GetAll<Note>())
                .Where(n => n.NotebookId == SelectedNotebookVM.Model.Id);

            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }
        #endregion

        public async Task UpdateProfilePicture()
        {
            // Load the profile picture from Azure Storage Blob
            var user = (await _db.GetAll<UserDetails>())
                .FirstOrDefault(u => u.UserId == App.LoggedInUser!.localId);

            var blob = await _azureBlobHelper.GetStreamFromBlobAsync(user.ProfilePictureBlobName, AppSettings.AzureStorage.UserPhotosContainerName);
            blob.Position = 0;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = blob;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            ProfilePicture = bitmap;
        }

        public async Task ViewProfile()
        {
            var user = (await _db.GetAll<UserDetails>())
                .FirstOrDefault(u => u.UserId == App.LoggedInUser!.localId);
            var profileWindow = _profileWindowFactory(user);
            profileWindow.ShowDialog();

            // Update the profile picture
            await UpdateProfilePicture();
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
