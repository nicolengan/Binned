using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Binned.Models
{
    public class Product
    {
        [Required, MinLength(3, ErrorMessage = "Enter at least 3 characters.")]
        [Display(Name = "Product ID")]
        public string ProductID { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, 200)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal ProductPrice { get; set; }

        [Required, MinLength(1)]
        public string ProductSize { get; set; }

        [Required, Range(1, 200)]
        public decimal ProductLength { get; set; }

        [Required, Range(1, 200)]
        public decimal ProductWaist { get; set; }

        [Required, MaxLength(1)]
        public string Availability { get; set; }

    }
}
