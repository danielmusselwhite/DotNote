using DotNote.Model;
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

            // todo - should UserDetails be injected in the ProfileVM instead of ProfileWindow so this logic can be placed in the ProfileVM's constructor instead?
            VM = vm;
            DataContext = VM;

            VM.Email = user.Email;
            VM.FirstName = user.FirstName;
            VM.LastName = user.LastName;
            VM.UserId = user.UserId;
            VM.ProfileId = user.Id;

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
    }
}
