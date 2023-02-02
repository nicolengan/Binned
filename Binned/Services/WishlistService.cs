using Binned.Model;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class WishlistService
    {
        private readonly MyDbContext _context;
        public WishlistService(MyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }
        public List<Wishlist> GetAll()
        {
            return _context.Wishlists.OrderBy(m => m.Id).ToList();
        }
        public async Task<Wishlist> GetWishlistByUserName(string userName)
        {
            var wishlist = _context.Wishlists
                        .Include(c => c.Items)
                            .ThenInclude(i => i.Product)
                        .FirstOrDefault(c => c.UserName == userName);

            if (wishlist != null)
                return wishlist;

            // if it is first attempt create new
            var newWishlist = new Wishlist
            {
                UserName = userName
            };

            _context.Wishlists.Add(newWishlist);
            await _context.SaveChangesAsync();
            return newWishlist;
        }

        // cart:
        public async Task<Cart> GetCartByUserName(string userName)
        {
            var cart = _context.Carts
                        .Include(c => c.Items)
                            .ThenInclude(i => i.Product)
                        .FirstOrDefault(c => c.UserName == userName);

            if (cart != null)
                return cart;

            // if it is first attempt create new
            var newCart = new Cart
            {
                UserName = userName
            };

            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();
            return newCart;
        }

        public WishlistItem? GetWishlistItemsById(int id)
        {
            WishlistItem? item = _context.WishlistItems.FirstOrDefault(x => x.Id.Equals(id));
            return item;

        }
        public WishlistItem? GetWishListItemByProductId(int id)
        {
            WishlistItem? item = _context.WishlistItems.FirstOrDefault(x => x.ProductId.Equals(id));
            return item;

        }

        public Product? GetProductById(int productId)
        {
            Product? item = _context.Products.FirstOrDefault(x => x.ProductId.Equals(productId));
            return item;

        }

        public async Task AddItem(string userName, int productId)
        {

            var wishlist = await GetWishlistByUserName(userName);
            Product product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            var itemexist = _context.WishlistItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (itemexist == null)
            {
                wishlist.Items.Add(
                   new WishlistItem
                   {
                       ProductId = productId,
                       Price = product.ProductPrice
                   }
                );
            }
            else
            {

            }
            _context.SaveChanges();
        }

        public async Task RemoveItem(int WishListId)
        {
            var wishlistitem = GetWishlistItemsById(WishListId);

            _context.WishlistItems.Remove(wishlistitem);

            await _context.SaveChangesAsync();
        }

        // Add to cart from wishlist button:

    }
}