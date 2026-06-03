using Azure.Storage.Blobs;
using DotNote.Configuration;
using DotNote.ViewModel;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace DotNote.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM VM;

        public NotesWindow()
        {
            InitializeComponent();

            VM = Resources["NotesVM"] as NotesVM; // assign to same instance of VM that's in XAML resources
            VM.SelectedNoteChanged += ViewModel_SelectedNoteChanged;

            var fontFamilies = Fonts.SystemFontFamilies
                .OrderBy(f => f.Source);

            // todo - this should be done with data binding and a view model, but for the sake of time I'm doing it in code behind
            cmbFontFamility.ItemsSource = fontFamilies;

            var fontSizes = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbFontSize.ItemsSource = fontSizes;
        }

        #region LifeCycle Methods

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (string.IsNullOrWhiteSpace(App.UserId))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();

                VM.GetNotebooks();
            }
        }

        #endregion

        #region EventHandlers
        private async void ViewModel_SelectedNoteChanged(object? sender, EventArgs e)
        {
            // Clear previous contents
            rtbContent.Document.Blocks.Clear();

            // Set the TextRange in the RichTextBox to the content of the selected note's RTF file if it exists
            if (!string.IsNullOrWhiteSpace(VM?.SelectedNote?.FileLocation))
            {
                // first download the file from azure storage, and save it locally
                string downloadPath = $"{VM.SelectedNote.Id}.rtf";
                await new BlobClient(new Uri(VM.SelectedNote.FileLocation)).DownloadToAsync(downloadPath);

                // then load that files contents into the Rich Text Box
                using (var fileStream = System.IO.File.OpenRead(downloadPath))
                {
                    var range = new TextRange(rtbContent.Document.ContentStart, rtbContent.Document.ContentEnd);
                    range.Load(fileStream, DataFormats.Rtf);
                }
            }
        }
        #endregion

        #region Top Menu Handlers
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Note View Handlers
        // todo - these should be done as commands in the NotesVM, but for the sake of time I'm doing it in code behind
        #region Note Toolbar Handlers
        private async void speechButton_Click(object sender, RoutedEventArgs e)
        {
            var region = AppSettings.SpeechToText.Region;
            var key = AppSettings.SpeechToText.SubscriptionKey;

            var speechConfig = SpeechConfig.FromSubscription(key, region);

            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (var recognizer = new SpeechRecognizer(speechConfig, audioConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync(); // Recognize speech for 15 seconds or until a pause is detected
                    rtbContent.Document.Blocks.Add(new Paragraph(new Run(result.Text))); // Append recognized text to the RichTextBox
                }
            }
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            bool isButtonChecked = button?.IsChecked ?? false;

            if(isButtonChecked) rtbContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else rtbContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            bool isButtonChecked = button?.IsChecked ?? false;

            if (isButtonChecked) rtbContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else rtbContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            bool isButtonChecked = button?.IsChecked ?? false;

            if (isButtonChecked) rtbContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else rtbContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }

        private void cmbFontFamility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = sender as ComboBox;
            var family = cmb?.SelectedItem as FontFamily;

            if (family == null) return;

            rtbContent.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, family);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb == null) return;

            if(double.TryParse(cmb.Text,out double fontSize))
            {
                rtbContent.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSize);
            }
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (VM.SelectedNote == null) return;

            string fileName = $"{VM.SelectedNote.Id}.rtf";
            string rtfFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);

            // first save the RTF locally
            using (var fileStream = System.IO.File.Create(rtfFilePath))
            {
                var range = new TextRange(rtbContent.Document.ContentStart, rtbContent.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }

            // then upload the RTF file and get the URL, and save that URL in the Notes db record
            VM.SelectedNote.FileLocation = await UploadBlob(rtfFilePath, fileName);
            await App.DbHelper.Update(VM.SelectedNote); // only update the Notes db record after successfully uploading the file, to avoid having a db record that points to a file that doesn't exist if the upload fails
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (VM.SelectedNote == null) return;

            string fileName = $"{VM.SelectedNote.Id}.rtf";
            string rtfFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);

            // first, delete the RTF file locally
            if (System.IO.File.Exists(rtfFilePath))
            {
                System.IO.File.Delete(rtfFilePath);
            }

            // then delete the file from Azure Storage, and if that succeeds, delete the Notes db record
            await DeleteBlob(rtfFilePath, fileName); 
            await App.DbHelper.Delete(VM.SelectedNote); // only delete the Notes db record if the file deletion was successful, to avoid orphaned files if the db record is deleted but the file isn't

            // refresh the notes list in the UI after deletion
            await VM.GetNotes();
        }
        #endregion

        #region RichTextBox Event Handlers
        private void rtbContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = new TextRange(rtbContent.Document.ContentStart, rtbContent.Document.ContentEnd).Text;

            // rich textbox includes a newline character at the end, so we trim it before counting characters
            int charCount = string.IsNullOrWhiteSpace(text)
                ? 0
                : text.TrimEnd('\r', '\n').Length;

            txtStatusMessage.Text = $"Character Count: {charCount}";
        }

        private void rtbContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = rtbContent.Selection.GetPropertyValue(Inline.FontWeightProperty);


            boldButton.IsChecked = selectedWeight != DependencyProperty.UnsetValue && selectedWeight.Equals(FontWeights.Bold); // if entire selection is bold, set button to checked
            underlineButton.IsChecked = rtbContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Underline; // if entire selection is underlined, set button to checked
            italicButton.IsChecked = rtbContent.Selection.GetPropertyValue(Inline.FontStyleProperty) as FontStyle? == FontStyles.Italic; // if entire selection is italic, set button to checked

            cmbFontFamility.SelectedItem = rtbContent.Selection.GetPropertyValue(Inline.FontFamilyProperty); // set font family combo box to match selection
            var fontSize = rtbContent.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text =
                fontSize == DependencyProperty.UnsetValue
                    ? ""
                    : fontSize.ToString(); // set font size combo box to match selection
        }
        #endregion

        #region Helper Methods
        private async Task<string> UploadBlob(string rtfFilePath, string fileName)
        {
            string connectionString = AppSettings.AzureStorage.ConnectionString;
            string containerName = AppSettings.AzureStorage.ContainerName;

            var containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExistsAsync(); // ensure container exists

            var blob = containerClient.GetBlobClient(fileName);
            await blob.UploadAsync(rtfFilePath, overwrite:true);
            return blob.Uri.ToString();
        }

        private async Task<string> DeleteBlob(string rtfFilePath, string fileName)
        {
            string connectionString = AppSettings.AzureStorage.ConnectionString;
            string containerName = AppSettings.AzureStorage.ContainerName;

            var containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExistsAsync(); // ensure container exists

            var blob = containerClient.GetBlobClient(fileName);
            var success = await blob.DeleteIfExistsAsync();

            if(!success) throw new Exception("Failed to delete blob from Azure Storage");

            return blob.Uri.ToString();
        }
        #endregion

        #endregion
    }
}
