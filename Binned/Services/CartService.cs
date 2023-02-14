using Binned.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

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

        public Cart? GetCartIDByUsername(string Username)
        {
            Cart? item = _context.Carts.FirstOrDefault(x => x.UserName.Equals(Username));
            return item;
        }

        public CartItem? GetCartItemByProductId(int id)
        {
            CartItem? item = _context.CartItems.FirstOrDefault(x => x.ProductId.Equals(id));
            return item;

        }

        public Product? GetProductById(int productId)
        {
            Product? item = _context.Products.FirstOrDefault(x => x.ProductId.Equals(productId));
            return item;

        }

        public Order? GetOrderIdByUsername(string UN)
        {
            //getting order status column
            Order? item = _context.Orders.FirstOrDefault(x => x.UserId.Equals(UN));
            return item;

        }
        public List<Order> GetOrderByUserId(string userId)
        {
            return _context.Orders
                .Include(i => i.Products)
                .Where(item => item.UserId == userId)
                .ToList();
        }

        public async Task<bool> AddItem(string userName, int productId)
        {

            var cart = await GetCartByUserName(userName);
            Product product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            var itemexist = _context.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            var Avail = product.Availability;

            if (Avail == "Y")
            {
                if (itemexist == null)
                {
                    cart.Items.Add(
                       new CartItem
                       {
                           ProductId = productId,
                           Price = product.ProductPrice
                       }
                    );
                    _context.SaveChanges();
                    return true;
                }
            }
            _context.SaveChanges();
            return false;
        }


        public void UpdateAvailabilityById(string id, string status)
        {
            var current = _context.Orders
                .Include(item => item.Products)
                 .FirstOrDefault(item => item.OrderId == id);
            var productList = current?.Products;

            foreach (var i in productList)
            {
                if (productList != null)
                {
                    i.Availability = status;

                    _context.SaveChanges();
                }
            }

        }

        public async Task RemoveItem(int CartItemId)
        {
            var cartitem = GetCartItemById(CartItemId);

            _context.CartItems.Remove(cartitem);

            await _context.SaveChangesAsync();
        }
        public async Task ClearCart(string userName)
        {
            var cart = await GetCartByUserName(userName);
            var cartitem = GetCartIDByUsername(userName);
            cart.Items.Clear();
            cartitem.Items.Clear();

            await _context.SaveChangesAsync();
        }
    }
}