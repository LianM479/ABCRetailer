using ABCRetailer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ABCRetailer.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly AzureTableStorage _tableHelper;

        public HomeController(IConfiguration configuration)
        {
            string connectionString = configuration["AzureTableStorage:ConnectionString"];

            _tableHelper = new AzureTableStorage(connectionString);
        }

       public async Task<IActionResult> Index()
        //public IActionResult Index()
        {
            List<Customer> customer = await _tableHelper.GetCustomersAsync();
            return View(customer);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products() {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
