using AutoMapper;
using DotNote.Model;
using DotNote.View;
using DotNote.ViewModel;
using DotNote.ViewModel.Helpers;
using DotNote.ViewModel.Helpers.DatabaseHelpers;
using DotNote.ViewModel.Helpers.StorageHelpers;
using DotNote.ViewModel.Login;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using static DotNote.ViewModel.Helpers.FirebaseAuthHelper;

namespace DotNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FirebaseResponse? LoggedInUser = null;

        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            
            services.AddLogging();

            ConfigureServices(services);

            Services = services.BuildServiceProvider();

            var mainWindow = Services.GetRequiredService<NotesWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Views
            services.AddSingleton<NotesWindow>();
            services.AddTransient<ProfileWindow>();
            services.AddSingleton<LoginWindow>();

            // ViewModels
            services.AddTransient<NotesVM>();
            services.AddTransient<ProfileVM>();
            services.AddTransient<LoginVM>();

            // AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserProfile>(); // profile for mapping User. UserDetails (db record), and the Firebase DTO (user auth record)
            });

            // Services
            services.AddSingleton<FirebaseAuthHelper>();
            services.AddSingleton<IDatabaseHelper, FirebaseDbHelper>();
            services.AddSingleton<FirebaseDbHelper>();
            services.AddSingleton<AzureBlobHelper>();

            // Factories for views with runtime args
            services.AddTransient<Func<UserDetails, ProfileWindow>>(sp =>
                user => ActivatorUtilities.CreateInstance<ProfileWindow>(sp, user));
        }
    }

}
