using Azure.Storage.Blobs;
using DotNote.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNote.ViewModel.Helpers.StorageHelpers
{
    public class AzureBlobHelper
    {
        public async Task DownloadBlobAsync(string fileName, string containerName, string downloadPath)
        {
            string connectionString = AppSettings.AzureStorage.ConnectionString;
            var containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExistsAsync(); // ensure container exists
            var blob = containerClient.GetBlobClient(fileName);
            await blob.DownloadToAsync(downloadPath);
        }

        public async Task DownloadBlobFromUriAsync(string blobUri, string downloadPath)
        {
            var blobClient = new BlobClient(new Uri(blobUri));
            await blobClient.DownloadToAsync(downloadPath);
        }

        public async Task<Stream> GetStreamFromBlobAsync(string fileName, string containerName)
        {
            string connectionString = AppSettings.AzureStorage.ConnectionString;
            var containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExistsAsync(); // ensure container exists
            var blob = containerClient.GetBlobClient(fileName);
            var memoryStream = new MemoryStream();
            await blob.DownloadToAsync(memoryStream);
            memoryStream.Position = 0; // Reset stream position to the beginning
            return memoryStream;
        }

        public async Task<string> UploadBlobAsync(string localFilePath, string fileName, string containerName)
        {
            string connectionString = AppSettings.AzureStorage.ConnectionString;

            var containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExistsAsync(); // ensure container exists

            var blob = containerClient.GetBlobClient(fileName);
            await blob.UploadAsync(localFilePath, overwrite: true);
            return blob.Uri.ToString();
        }

        public async Task<bool> DeleteBlobAsync(string fileName, string containerName)
        {
            string connectionString = AppSettings.AzureStorage.ConnectionString;

            var containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExistsAsync(); // ensure container exists

            var blob = containerClient.GetBlobClient(fileName);
            return await blob.DeleteIfExistsAsync();
        }
    }
}
