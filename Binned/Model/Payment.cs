using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Binned.Model
{
    public class Payment
    {
        [Required]
        public int PaymentId { get; set; }

        [Required]
        public string Status { get; set; }

        [Range(0, 1e6)]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        public int OrderForeignKey { get; set; }
        public Order? Order { get; set; }
    }
}
