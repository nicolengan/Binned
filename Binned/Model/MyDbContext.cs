using Binned.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Win32;
using System.Reflection.Metadata;
public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<BinnedUser>
{
    public void Configure(EntityTypeBuilder<BinnedUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);
    }
}
namespace Binned.Model
{


    public class MyDbContext : IdentityDbContext<BinnedUser>
    {

        /*public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
        {
        }*/

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }

        private readonly IConfiguration _configuration;
        //public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }
        public MyDbContext(IConfiguration configuration, DbContextOptions<MyDbContext> options) : base(options)
        {
            _configuration = configuration;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString");
            optionsBuilder
            .UseSqlServer(connectionString);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

    }
}
