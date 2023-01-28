using Binned.Model;
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
            return _context.Orders.OrderBy(d => d.OrderId).ToList();
        }

        public Order? GetOrderById(int id)
        {
            Order? order = _context.Orders.FirstOrDefault(
            x => x.OrderId.Equals(id));
            return order;
        }

        public List<Order> GetOrderByUserId(string userId)
        {
            return _context.Orders
                    .Where(b => b.UserId.Contains(userId))
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
        public void UpdateOrderById(int id, string status)
        {
            var current = _context.Orders.FirstOrDefault(item => item.OrderId == id);
            if (current != null)
            {
                current.Status = status;
                _context.SaveChanges();
            }
        }
    }
}