using Azure;
using Azure.Data.Tables;
using System;
using System.ComponentModel.DataAnnotations;

namespace ABCRetailer.Models
{
    public class Customer : ITableEntity
    {
        public string PartitionKey { get; set; } = "region";
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        //public Customer() {}
        [Required]
        public string? customerName { get; set; }
        public string? customerSurname { get; set; }
        public string? customerEmail { get; set; }  
    }
}
