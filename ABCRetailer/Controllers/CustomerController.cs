using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using System.Threading.Tasks;
using ABCRetailer.Controllers;
using ABCRetailer.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace ABCRetailer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AzureTableStorage _tableHelper;

        //public CustomerController(IConfiguration configuration)
        public CustomerController(AzureTableStorage tableHelper)
        {   
            _tableHelper = tableHelper;

            //string? connectionString = configuration["AzureTableStorage:ConnectionString"];

            //if (string.IsNullOrEmpty(connectionString))
            //{
                //throw new InvalidOperationException("Azure Table Storage connection string is missing.");
           // }
           // _tableHelper = new AzureTableStorage(connectionString, "Customers");
        
        }
        
        public async Task<IActionResult> Index()
        {
            var customer = await _tableHelper.GetCustomersAsync();
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(String name, string surname, string email)
        {
            var customer = new Customer
            {
                PartitionKey = "region",
                RowKey = Guid.NewGuid().ToString(),
                customerName = name,
                customerSurname = surname,
                customerEmail = email
            };

            await _tableHelper.AddCustomerAsync(customer);
            // var tableClient = GetTableClient();
            //await _tableHelper.AddEntityAsync(customer);
            return RedirectToAction("Index");
        }
        

    }
}
