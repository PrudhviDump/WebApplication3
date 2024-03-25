using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class UpdateProductModel
    {
        public int ProductId { get; set; }
        
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(50, ErrorMessage = "Product name must be between 1 and 50 characters.", MinimumLength = 1)]
        public string ProductName { get; set; }

        [Url(ErrorMessage = "Invalid image URL.")]
        public string ProductImageUrl { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0.")]
        public decimal ProductPrice { get; set; }

        [StringLength(500, ErrorMessage = "Product details must be less than 500 characters.")]
        public string ProductDetails { get; set; }
    }
}
