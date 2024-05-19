using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StorageApp.Dtos.Items;
using StorageApp.Models;
using StorageApp.Utility;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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
        public IActionResult Index(string itemName = "", string supplierName = "", decimal minPrice = 1, decimal maxPrice = 1000000)
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

            // Have everything serialized, so that a report can easily be generated any time the user wishes
            ViewData["jsonDto"] = JsonConvert.SerializeObject(dto);

            return View(dto);
        }


        /// <summary>
        /// Generates a report file and returns it to the client
        /// </summary>
        /// <param name="dtoJson">The json that has to be deserialized to a ItemsIndexDto so that all information can be extracted that is needed for the report</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GenerateReport(string dtoJson)
        {
            ItemsIndexDto dto = JsonConvert.DeserializeObject<ItemsIndexDto>(dtoJson);

            if (dto == null)
            {
                return RedirectToAction("Error");
            }

            DataTable info = ReportGenerator.GenerateInfoDataTable(dto);
            DataTable report = ReportGenerator.GenerateReportDataTable(dto);

            return ReportGenerator.GenerateExcelWorkbook(info, report);
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
