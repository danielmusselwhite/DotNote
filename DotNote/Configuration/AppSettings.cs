using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNote.Configuration
{
    public static class AppSettings
    {
        public static SpeechToTextSettings SpeechToText { get; }
        public static FirebaseSettings Firebase { get; }

        static AppSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            SpeechToText = configuration
                .GetSection("SpeechToText")
                .Get<SpeechToTextSettings>()
                ?? new SpeechToTextSettings();

            Firebase = configuration
                .GetSection("Firebase")
                .Get<FirebaseSettings>()
                ?? new FirebaseSettings();
        }
    }
}
