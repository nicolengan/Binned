using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Binned.Model
{
    public class Product
    {
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Required, MaxLength(50)]
        public string ProductName { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string ProductDescription { get; set; } = string.Empty;

        [Range(1, 200)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal ProductPrice { get; set; }

        [Required, MinLength(1)]
        public string ProductSize { get; set; }

        [Required, MaxLength(1)]
        public string Availability { get; set; }

        [MaxLength(50)]
        public string? ImageURL { get; set; }

        public ICollection<Order>? Orders { get; set; }

    }
}
