using Microsoft.AspNetCore.Mvc;
using StorageApp.Dtos.Items;
using StorageApp.Models;
using System.Diagnostics;

namespace StorageApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string itemName="", string supplierName="", decimal minPrice = 1, decimal maxPrice = 1000000)
        {
            // The idea is to keep adding filter statements to the query, before finally executing it by calling .ToList()
            IQueryable<Item> currentQuery = _context.Items;

            // Filter by itemName
            if (!string.IsNullOrWhiteSpace(itemName)) 
            {
                itemName = itemName.Trim();
                currentQuery = currentQuery.Where(i => i.Name.Contains(itemName));
            }
            // Filter by supplierName
            if (!string.IsNullOrWhiteSpace(supplierName))
            {
                supplierName = supplierName.Trim();
                currentQuery = currentQuery.Where(i => i.Supplier.Contains(supplierName));
            }

            // Filter by price
            currentQuery = currentQuery.Where(i => i.Price >= minPrice && i.Price <= maxPrice);

            // Get final result
            List<Item> allFilteredItems = currentQuery.ToList();

            // Give a dto to the view, so that the search terms get remembered
            var dto = new ItemsIndexDto() 
            {
                AllItems = allFilteredItems,
                ItemName = itemName,
                SupplierName = supplierName,
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
