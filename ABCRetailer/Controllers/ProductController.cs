using ABCRetailer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using ABCRetailer.Models.Services;

namespace ABCRetailer.Controllers
{
    public class ProductController : Controller
    {
        private readonly AzureTableStorage _tableStorage;
        private readonly AzureBlobStorage _blobStorage;

        public ProductController(AzureTableStorage tableStorage, AzureBlobStorage blobStorage)
        {
            _tableStorage = tableStorage;
            _blobStorage = blobStorage;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _tableStorage.GetProductsAsync();

            if (products == null || !products.Any())
            {
                ViewBag.Message = "No products available.";
                // ViewBag.Products = new List<Product>(); // Ensure it's never null
            }
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product products, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    PartitionKey = "category",
                    RowKey = Guid.NewGuid().ToString(),
                    productTitle = products.productTitle,
                    productAuthor = products.productAuthor,
                    productDescription = products.productDescription,
                    productPrice = products.productPrice,
                    ImageUrl = products.ImageUrl
                };
                if (imageFile != null)
                {
                    using var stream = imageFile.OpenReadStream();
                    product.ImageUrl = await _blobStorage.UploadImageAsync(imageFile.FileName, stream);
                }
                await _tableStorage.AddProductAsync(product);

               // var productList = await _tableStorage.GetProductsAsync();
                //Console.WriteLine("Product added successfully: " + product.RowKey);
                return RedirectToAction("Index", "Product");
                //return View("~/Views/Home/Products.cshtml");
                //return View("Product", productList);
            }
            return View(products);
        }
        //public async Task<IActionResult> Products()
        //{
          //  var products = await _tableStorage.GetProductsAsync();
            //return View(products);
        //}
    }
}
