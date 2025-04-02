using Azure.Storage.Files.Shares;
using System.Text;
using System;
using System.IO;
using System.Threading.Tasks;


namespace ABCRetailer.Models.Services

{
    public class FileStorage
    {
        private readonly ShareClient _shareClient;
        private readonly ShareDirectoryClient _directoryClient;

        public FileStorage(string connectionString, string shareName)
        {
            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists(); // Ensure file share exists

            _directoryClient = _shareClient.GetRootDirectoryClient();
        }
        //public async Task WriteLogAsync(string logMessage)
        //{
        //string fileName = $"log_{DateTime.UtcNow:yyyyMMdd}.txt";
        //ShareFileClient fileClient = _directoryClient.GetFileClient(fileName);

        //if (!fileClient.Exists())
        //{
        //   await fileClient.CreateAsync(1024); // Create file if it doesn't exist
        // }

        // byte[] messageBytes = Encoding.UTF8.GetBytes(logMessage + Environment.NewLine);
        // using MemoryStream stream = new MemoryStream(messageBytes);

        //   await fileClient.UploadRangeAsync(new Azure.HttpRange(0, messageBytes.Length), stream);
        // }
        public async Task UploadLogAsync(string logMessage)
        {
            string fileName = $"log-{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
            ShareFileClient fileClient = _directoryClient.GetFileClient(fileName);

            byte[] fileContents = Encoding.UTF8.GetBytes(logMessage);
            using (MemoryStream stream = new MemoryStream(fileContents))
            {
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);
            }

        }
    }
}



