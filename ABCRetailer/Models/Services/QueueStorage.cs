using Azure.Storage.Queues;
using System.Text;
using System;
using System.Threading.Tasks;

namespace ABCRetailer.Models.Services
{
    public class QueueStorage
    {
        private readonly QueueClient _queueClient;

        public QueueStorage(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists(); // Ensure queue exists
        }

        public async Task SendMessageAsync(string message)
        {
            if (_queueClient.Exists())
            {
                await _queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
            }
        }
    }
}