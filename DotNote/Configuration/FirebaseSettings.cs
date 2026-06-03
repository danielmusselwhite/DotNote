using System;
using System.Collections.Generic;
using System.Text;

namespace DotNote.Configuration
{
    public class FirebaseSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string AuthDomain { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string StorageBucket { get; set; } = string.Empty;
        public string MessagingSenderId { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public string MeasurementId { get; set; } = string.Empty;
    }
}
