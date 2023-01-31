namespace Binned.Model
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public List<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }
}
