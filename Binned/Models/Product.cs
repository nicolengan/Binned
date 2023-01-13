using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Binned.Models
{
    public class Product
    {
        [Required]
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public string ProductDescription { get; set; } = string.Empty;

        [Required]
        public string Size { get; set; } = string.Empty;

        [Required]
        public string Length { get; set; } = string.Empty;

        [Required]
        public string Waist { get; set; } = string.Empty;

        [Required]
        public string Availability { get; set; } = string.Empty;

        [Required, Range(0, 15000)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; }

    }
}
