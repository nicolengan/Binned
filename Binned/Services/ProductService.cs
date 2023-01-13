using Binned.Models;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class ProductService
    {
        private readonly MyDbContext _context;

        public ProductService(MyDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.OrderBy(m => m.ProductName).ToList();
        }

        public Product? GetProductById(int id)
        {
            Product? product = _context.Products.FirstOrDefault(x => x.ProductId.Equals(id));
            return product;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

    }
}
