using Binned.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Win32;
using System.Reflection.Metadata;

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
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Order> Orders { get; set; }
<<<<<<< Updated upstream
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Login> Login { get; set; } = null;
        public DbSet<Register> Register { get; set; } = null;
=======
        //public DbSet<Login> Login { get; set; }
        //public DbSet<Register> Register { get; set; }
>>>>>>> Stashed changes
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(b => b.Payment)
                .WithOne(i => i.Order)
                .HasForeignKey<Payment>(b => b.OrderForeignKey);
        }
    }




}
