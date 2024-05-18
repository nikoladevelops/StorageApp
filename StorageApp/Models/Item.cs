using System.ComponentModel.DataAnnotations;

namespace StorageApp.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [MaxLength(30, ErrorMessage = "The Name cannot be longer than 30 characters.")]
        [RegularExpression(@"^(?!\s)(?!.*\s$).*$", ErrorMessage = "Name cannot contain only white spaces. It also cannot start or end with a white space.")]

        public string Name { get; set; }

        [Required(ErrorMessage = "The Quantity field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The Quantity must be a non-negative integer.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0d, double.MaxValue, ErrorMessage = "The Price must be a non-negative number.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The Supplier field is required.")]
        [MaxLength(30, ErrorMessage = "The Supplier cannot be longer than 30 characters.")]
        [RegularExpression(@"^(?!\s)(?!.*\s$).*$", ErrorMessage = "Supplier cannot contain only white spaces. It also cannot start or end with a white space.")]
        public string Supplier { get; set; }
    }
}
