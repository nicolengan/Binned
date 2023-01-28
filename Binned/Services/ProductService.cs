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
            return product;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        {
        }

    }
}
