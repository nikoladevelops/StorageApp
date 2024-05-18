using Microsoft.AspNetCore.Mvc;
using StorageApp.Models;
using System.Diagnostics;

namespace StorageApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly ApplicationDbContext _context;

        public ItemsController(ILogger<ItemsController> logger, ApplicationDbContext context)
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
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);
            if (item == null) 
            {
                return RedirectToAction("Error");
            }

            return View(item);
        }


        [HttpPost]
        public IActionResult Create(Item newItem)
        {
            if (!ModelState.IsValid) 
            {
                return View("Add");
            }

            _context.Items.Add(newItem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(Item newItem)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }

            _context.Items.Update(newItem);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == id);
            if (item == null) 
            {
                return RedirectToAction("Error");
            }
            _context.Items.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
