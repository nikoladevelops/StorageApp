using Microsoft.AspNetCore.Mvc;
using StorageApp.Models;
using System.Diagnostics;

namespace StorageApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Additional parameters for filtering
        [HttpGet]
        public IActionResult Index()
        {
            var allItems = _context.Items.ToList();
            return View(allItems);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // TODO creating a new Item
            return View();
        }

        [HttpGet]
        public IActionResult Update()
        {
            // TODO update an item by receiving a dto containing the updated info
            return View();
        }

        [HttpPost]
        public IActionResult Delete()
        {
            // TODO deleting 
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
