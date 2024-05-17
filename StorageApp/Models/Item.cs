using System.ComponentModel.DataAnnotations;

namespace StorageApp.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [MaxLength(30, ErrorMessage = "The Name cannot be longer than 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Quantity field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The Quantity must be a non-negative integer.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The Price must be a non-negative value.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "The Supplier field is required.")]
        [MaxLength(30, ErrorMessage = "The Supplier cannot be longer than 30 characters.")]
        public string Supplier { get; set; }
    }
}
