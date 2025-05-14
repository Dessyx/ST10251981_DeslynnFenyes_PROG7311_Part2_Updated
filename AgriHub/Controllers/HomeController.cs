using System.Diagnostics;
using AgriHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgriHub.Controllers
{
    //--------------------------------------------------------------------------------------------------
    // Controller that handles the display of the landing page
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //-----------------------------------------------------------------------------------------------
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //-----------------------------------------------------------------------------------------------
        // Displays the landing page
        public IActionResult Index()
        {
            return View();
        }

        //-----------------------------------------------------------------------------------------------
        public IActionResult Privacy()
        {
            return View();
        }

        //-----------------------------------------------------------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
// -------------------------------------------<<< End of File >>>-----------------------------------------