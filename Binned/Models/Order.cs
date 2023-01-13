using System.ComponentModel.DataAnnotations.Schema;

namespace Binned.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [Column(TypeName = "decimal(7,2)")]
        public decimal TotalPrice { get; set; }
    }
}
