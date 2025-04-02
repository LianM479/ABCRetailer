using ABCRetailer.Models;
using ABCRetailer.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ABCRetailer.Controllers
{
    public class OrderController : Controller
    {
        private readonly QueueStorage _queueStorage;
        private readonly FileStorage _fileStorage;

            public OrderController(IConfiguration configuration)
        {
            string? connectionString = configuration["AzureStorage:ConnectionString"];
            string queueName = configuration["AzureStorage:QueueName"];
            string filseShareName = configuration["AzureStorage:FileShareName"];


            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Azure Storage connection string is missing from configuration.");
            }

            _queueStorage = new QueueStorage(connectionString, configuration["AzureStorage:QueueName"]);
            _fileStorage = new FileStorage(connectionString, filseShareName);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Order()); // return empty order model
        }

        [HttpPost]
        // public IActionResult Index()
        public async Task<IActionResult> Create(Order order)
        {
            if (order == null)
                return BadRequest("Invalid order data.");
            //check orderId is not null
            if (string.IsNullOrEmpty(order.OrderId))
            {
                order.OrderId = Guid.NewGuid().ToString();
            }

            //check products is not null
            if (order.Products == null)
            {
                order.Products = new List<Product>();
            }
            Console.WriteLine($"OrderId: {order.OrderId}");
            Console.WriteLine($"CustomerName: {order.CustomerName}");
            Console.WriteLine($"TotalPrice: {order.TotalPrice}");
            Console.WriteLine($"Products Count: {order.Products.Count}");

            // Convert order to JSON
            string orderJson = JsonConvert.SerializeObject(order, Formatting.Indented);

            // Send order details to Azure Queue
            await _queueStorage.SendMessageAsync($"New Order: {orderJson}");

            // Log the order in Azure Files
            await _fileStorage.UploadLogAsync($"Order {order.OrderId} placed by {order.CustomerName}");

            //return View("Create", Create);
            TempData["SuccessMessage"] = "Order placed successfullt!";
            return RedirectToAction("Create");
        }

        //[HttpPost]
       // public async Task<IActionResult> ProcessOrder(string orderDetails)
        ///{
           // if (string.IsNullOrEmpty(orderDetails))
             //   return BadRequest("Order details cannot be empty");

            // Send order details to Azure Queue
            //await _queueStorage.SendMessageAsync($"Processing order: {orderDetails}");

            // Log order processing
            //await _fileStorage.WriteLogAsync($"Order Processed: {orderDetails} - {DateTime.UtcNow}");

            //return Ok("Order processed successfully!");
        //}
    }
} 
