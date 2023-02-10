using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Binned.Model
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime ExpireDate { get; set; }
        public double Discount { get; set; }
    }
}
