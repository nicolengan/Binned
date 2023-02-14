using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
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
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [AllowNull]
        public DateTime? ShipDate { get; set; }

        public string Status { get; set; }

        [Range(0, 1e6)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Amount { get; set; }

        public ICollection<Product> Products { get; set; }

        public string Address { get; set; }
        [AllowNull]
        public string? Address2 { get; set; }
        public int PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PromoCode? PromoCode { get; set; }

    }
}
