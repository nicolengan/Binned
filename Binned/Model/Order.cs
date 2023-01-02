using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Binned.Model
{
    public class Order
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? ShipDate { get; set; }

        [Required]
        public bool PaymentStatus { get; set; } = false;

        public Payment? Payment { get; set; }
    }
}
