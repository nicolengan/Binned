using Binned.Models;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class OrderService
    {
        private readonly MyDbContext _context;

        public OrderService(MyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _context.Orders
                            .Where(o => o.UserName == userName)
                            .ToListAsync();

            return orderList;
        }
    }
}