using Binned.Model;
namespace Binned.Services
{
    public class ProductService
    {

        private readonly MyDbContext _context;
        public ProductService(MyDbContext context)
        {
            _context = context;
        }
        public List<Product> GetAvailProducts()
        {
            var availProducts = from product in _context.Products where product.Availability == "Y" select product;
            return availProducts.ToList();
        }

        public List<Product> GetAll()
        {
            return _context.Products.OrderBy(m => m.ProductName).ToList();
        }
        public Product? GetProductById(int id)
        {
            Product? product = _context.Products.FirstOrDefault(
            x => x.ProductId.Equals(id));
            return product;
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
