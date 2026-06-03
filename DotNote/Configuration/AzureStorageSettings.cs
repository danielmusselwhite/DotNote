using System;
using System.Collections.Generic;
using System.Text;

namespace DotNote.Configuration
{
    public class AzureStorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
