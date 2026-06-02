using DotNote.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotNote.View.UserControls
{
    /// <summary>
    /// Interaction logic for DisplayNote.xaml
    /// </summary>
    public partial class DisplayNote : UserControl
    {
        #region Dependency Properties
        // This is a dependency property for the Note that will be displayed in this control. It allows us to bind a Note object to this control and have it update the UI when the Note changes.
        public static readonly DependencyProperty NoteDependencyProperty = DependencyProperty.Register(
            "Note",
            typeof(Note),
            typeof(DisplayNote),
            new PropertyMetadata(null, SetValues)); // if the Note property changes, the SetValues method will be called to update the UI with the new Note data.

        // This is the CLR wrapper for the Note dependency property. It allows us to get and set the Note property in code, while still allowing it to be used as a dependency property in XAML.
        public Note Note
        {
            get { return (Note) GetValue(NoteDependencyProperty); }
            set { SetValue(NoteDependencyProperty, value); }
        }
        #endregion

        public DisplayNote()
        {
            InitializeComponent();
        }

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNote NoteUserControl = d as DisplayNote;
            if (NoteUserControl == null) return;

            NoteUserControl.DataContext = NoteUserControl.Note;
        }

    }
}
