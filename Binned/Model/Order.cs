using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Binned.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        [AllowNull]
        public string ProductId { get; set; }
        [AllowNull]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [AllowNull]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [AllowNull]
        public DateTime? ShipDate { get; set; }

        public bool PaymentStatus { get; set; } = false;

        public Payment? Payment { get; set; }
    }
}
