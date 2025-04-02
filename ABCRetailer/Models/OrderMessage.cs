using Microsoft.AspNetCore.Mvc;

namespace ABCRetailer.Models
{
    public class OrderMessage : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
