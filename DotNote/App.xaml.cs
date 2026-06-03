using DotNote.ViewModel.Helpers.DatabaseHelpers;
using System.Windows;

namespace DotNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string UserId { get; set; } = string.Empty;
        public static IDatabaseHelper DbHelper { get; private set; } = new FirebaseDbHelper();
    }

}
