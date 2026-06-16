using DotNote.Configuration;
using DotNote.Model;
using DotNote.ViewModel.Helpers.StorageHelpers;
using DotNote.ViewModel.Login;
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
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private readonly ProfileVM VM;

        public ProfileWindow(ProfileVM vm, UserDetails user)
        {
            InitializeComponent();

            VM = vm;
            DataContext = VM;

            Loaded += async (_, _) => await VM.InitializeAsync(user); // after loading window, asyncronously initialize the view model with user details
            VM.ProfileUpdateFinished += VM_ProfileUpdateFinished;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VM_ProfileUpdateFinished(object? sender, EventArgs e)
        {
            Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            VM.ProfileUpdateFinished -= VM_ProfileUpdateFinished;
            base.OnClosed(e);
        }
    }
}
