using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace Binned.Model
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; }

        public bool PaymentStatus { get; set; } = false;
        [Range(0, 1e6)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Amount { get; set; }

        public ICollection<Product> Products { get; set; }

        public ShippingInfo ShippingInfo { get; set; }
    }
}
