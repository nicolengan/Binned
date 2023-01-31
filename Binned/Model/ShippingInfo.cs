using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;

namespace Binned.Model
{
    public class ShippingInfo
    {
        public string ShippingInfoId { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [AllowNull]
        public DateTime? ShipDate { get; set; }
        public string UserId { get; set; }
        public string StreetName { get; set; }
        [AllowNull]
        public string Block { get; set; }
        public string UnitNumber { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public Boolean Default { get; set; }    

        public ICollection<Order> Orders { get; set; }
    }
}
