using System;
using System.Collections.Generic;
using System.Text;

namespace DotNote.Configuration
{
    public class AzureStorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string NotesContainerName { get; set; } = string.Empty;
        public string UserPhotosContainerName { get; set; } = string.Empty;
    }
}
