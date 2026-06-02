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
    /// Interaction logic for DisplayNotebook.xaml
    /// </summary>
    public partial class DisplayNotebook : UserControl
    {
        #region Dependency Properties
        // This is a dependency property for the Notebook that will be displayed in this control. It allows us to bind a Notebook object to this control and have it update the UI when the Notebook changes.
        public static readonly DependencyProperty NotebookDependencyProperty = DependencyProperty.Register(
            "Notebook",
            typeof(Notebook),
            typeof(DisplayNotebook),
            new PropertyMetadata(null, SetValues)); // if the Notebook property changes, the SetValues method will be called to update the UI with the new Notebook data.

        // This is the CLR wrapper for the Notebook dependency property. It allows us to get and set the Notebook property in code, while still allowing it to be used as a dependency property in XAML.
        public Notebook Notebook
        {
            get { return (Notebook) GetValue(NotebookDependencyProperty); }
            set { SetValue(NotebookDependencyProperty, value); }
        }
        #endregion

        public DisplayNotebook()
        {
            InitializeComponent();
        }

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNotebook notebookUserControl = d as DisplayNotebook;
            if (notebookUserControl == null) return;

            notebookUserControl.DataContext = notebookUserControl.Notebook;
        }

    }
}
