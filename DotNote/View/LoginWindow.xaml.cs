using DotNote.ViewModel;
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
using System.Windows.Shapes;

namespace DotNote.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginVM VM;

        public LoginWindow()
        {
            InitializeComponent();
        
            VM = Resources["LoginVM"] as LoginVM;
            VM.Authenticated += ViewModel_Authenticated;
        }

        #region Event Handlers
        private void ViewModel_Authenticated(object sender, EventArgs e)
        {
            Close(); // just close this window so it stops blocking the main Notes window; which when activated will check if the user is authenticated and either load the data for the authed user or show this login window again
        }
        #endregion
    }
}
