using Microsoft.AspNetCore.Mvc;

namespace ABCRetailer.Controllers
{
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
