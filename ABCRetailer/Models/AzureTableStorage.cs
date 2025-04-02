using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABCRetailer.Models
{
    public class AzureTableStorage
    {
        //for customer
        private readonly TableClient _tableClient;
        //for product
        private readonly TableClient _productTable;
        
        private readonly TableServiceClient _serviceClient;

        public AzureTableStorage(string connectionString)
        {
            _serviceClient = new TableServiceClient(connectionString);
            //_tableClient = new TableClient(connectionString, Customer);

            //initializing customer table
            _tableClient = _serviceClient.GetTableClient("Customer");
            _tableClient.CreateIfNotExists();
            //initialize product table
            _productTable = _serviceClient.GetTableClient("Product");
            _productTable.CreateIfNotExists();
        }

        //add customer to azure table storage
        public async Task AddCustomerAsync(Customer customer)
        {
            //customer.PartitionKey = "region";
            //customer.RowKey = customer.RowKey;
            await _tableClient.AddEntityAsync(customer);
        }
        public async Task<List<Customer>> GetCustomersAsync()
        {
            var customers = new List<Customer>();
            await foreach (var customer in _tableClient.QueryAsync<Customer>())
            {
                //customer.Add(entity);
                customers.Add(customer);
            }
            return customers;
        } 

        //add product
        public async Task AddProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.PartitionKey))
            {
                product.PartitionKey = "category"; // Default partition key
            }

            if (string.IsNullOrEmpty(product.RowKey))
            {
                product.RowKey = Guid.NewGuid().ToString(); // Generate a unique RowKey
            }

            await _productTable.AddEntityAsync(product);
            Console.WriteLine($"Product Added: {product.productTitle} - {product.RowKey}");
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            var products = new List<Product>();

            try
            {
                await foreach (var entity in _productTable.QueryAsync<Product>())
                {
                    products.Add(entity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retreiving products: {ex.Message}");
            }
            Console.WriteLine($"Returning {products.Count} products");
            return products;
        }
    }
}
