using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Binned.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string Status { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? ShipDate { get; set; }

        public bool PaymentStatus { get; set; } = false;

        public Payment? Payment { get; set; }
    }
}
