using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Binned.Model
{
    public class WishlistItem
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
