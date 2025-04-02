using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Data.Tables;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ABCRetailer.Models.Services
{
    public class AzureBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        public AzureBlobStorage(string connectionString, string containerName= "product-images")
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadImageAsync(string fileName, Stream fileStream)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, true);
            //return url for uploaded image
            return blobClient.Uri.ToString();
         }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file), "File cannot be null.");

            var blobClient = _containerClient.GetBlobClient(file.FileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            return blobClient.Uri.ToString();
        }

            // public async Task<Stream> DownloadFileAsync(string fileName)
            // {
            // var blobClient = _containerClient.GetBlobClient(fileName);
            // BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
            //return downloadResult.Content;
            // }
        }
}
