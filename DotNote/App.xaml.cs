using System.Configuration;
using System.Data;
using System.Windows;

namespace DotNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string UserId { get; set; } = string.Empty;
    }

}
