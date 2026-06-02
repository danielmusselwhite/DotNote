using System;
using System.Collections.Generic;
using System.Text;

namespace DotNote.Configuration
{
    public class SpeechToTextSettings
    {
        public string SubscriptionKey { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
