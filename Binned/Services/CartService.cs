using Binned.Models;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class CartService
    {
        private readonly MyDbContext _context;

        public CartService(MyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }
        public List<Cart> GetAll()
        {
            return _context.Carts.OrderBy(m => m.Id).ToList();
        }
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
        public CartItem? GetCartItemById(int id)
        {
            CartItem? item = _context.CartItems.FirstOrDefault(x => x.Id.Equals(id));
            return item;

        }

        public async Task AddItem(string userName, int productId)
        {

            var cart = await GetCartByUserName(userName);

            cart.Items.Add(
               new CartItem
               {
                   ProductId = productId,
                   Price = _context.Products.FirstOrDefault(p => p.ProductId == productId).Price
               }
           );
            _context.SaveChanges();
        }

        public async Task RemoveItem(int CartItemId)
        {
            var cartitem = GetCartItemById(CartItemId);

            _context.CartItems.Remove(cartitem);

            await _context.SaveChangesAsync();
        }
    }
}
