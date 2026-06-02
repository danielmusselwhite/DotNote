using DotNote.Configuration;
using DotNote.ViewModel;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotNote.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        public NotesWindow()
        {
            InitializeComponent();
        }

        #region Top Menu Handlers
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Note View Handlers
        #region Note Toolbar Button Click Handlers
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
        }
        #endregion

        #endregion
    }
}
