using Azure;
using Azure.Data.Tables;
using System;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;



namespace ABCRetailer.Models
{
    public class Product : ITableEntity
    {
            public string PartitionKey { get; set; } = "category";
            public string? RowKey { get; set; } //book ID
            public ETag ETag { get; set; }
            public DateTimeOffset? Timestamp { get; set; }


        // public Product() { }
        [Required]
        public string? productTitle { get; set; }
        public string? productAuthor { get; set; }
        [Required]
        public string? productDescription { get; set; }
        public decimal productPrice { get; set; }
        public string? ImageUrl { get; set; }

    }
}
