using Binned.Model;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class OrderService
    {
        private readonly MyDbContext _context;

        public OrderService(MyDbContext context)
        {
            _context = context;
        }
        public List<Order> GetAll()
        {
            return _context.Orders
                .Include(i => i.Products)
                .OrderBy(d => d.OrderId)
                .ToList();
        }
        public Order? GetOrderById(string id)
        {
            Order? order = _context.Orders
                .Include(i => i.Products)
                .Include(s => s.ShippingInfo)
                .FirstOrDefault(x => x.OrderId.Equals(id));
            return order;
        }

        public List<Order> GetOrderByUserId(string userId)
        {
            return _context.Orders
                .Include(i => i.Products)
                .Include(s => s.ShippingInfo)
                .Where(item => item.UserId == userId)
                .ToList();
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order); // update vs attatched update updates all even if u only change one code, attatched only updates that one column 
            _context.SaveChanges();
        }
        public void UpdateStatusById(string id, string status)
        {
            var current = _context.Orders.FirstOrDefault(item => item.OrderId == id);
            if (current != null)
            {
                current.Status = status;
                _context.SaveChanges();
            }
        }
        public void CalculateTotal(string id)
        {
            var current = _context.Orders
                .Include(item => item.Products)
                .Include(s => s.ShippingInfo)
                .FirstOrDefault(item => item.OrderId == id);
            var productList = current?.Products;
            decimal total = 0;
            if (productList != null)
            {
                foreach (var i in productList)
                {
                    total += i.ProductPrice;
                }

            }
            if (current != null)
            {
                current.Amount = total;
                _context.SaveChanges();
            }
        }

    }
}