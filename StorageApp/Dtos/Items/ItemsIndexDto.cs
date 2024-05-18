using StorageApp.Models;

namespace StorageApp.Dtos.Items
{
    public class ItemsIndexDto
    {
        public List<Item> AllItems { get; set; }
        public string ItemName { get; set; }
        public string SupplierName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
