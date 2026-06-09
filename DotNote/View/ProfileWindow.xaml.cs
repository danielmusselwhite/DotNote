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

            VM = vm;
            DataContext = VM;

            VM.Email = user.Email;
            VM.FirstName = user.FirstName;
            VM.LastName = user.LastName;
        }
        

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
