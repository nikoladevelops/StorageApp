using Microsoft.AspNetCore.Mvc;
using StorageApp.Dtos.Items;
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

        [HttpGet]
        public IActionResult Index(string searchTerm="", decimal minPrice = 1, decimal maxPrice = 1000000)
        {
            IQueryable<Item> currentQuery = _context.Items;

            if (!string.IsNullOrWhiteSpace(searchTerm)) 
            {
                searchTerm = searchTerm.Trim();
                currentQuery = currentQuery.Where(i => i.Name.Contains(searchTerm));
            }

            currentQuery = currentQuery.Where(i => i.Price >= minPrice && i.Price <= maxPrice);

            List<Item> allFilteredItems = currentQuery.ToList();

            var dto = new ItemsIndexDto() 
            {
                AllItems = allFilteredItems,
                SearchTerm = searchTerm,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            return View(dto);
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
