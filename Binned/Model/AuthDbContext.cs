using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Binned.ViewModels;

namespace Account.Model
{



    public class AuthDbContext : IdentityDbContext
    {
        private readonly IConfiguration _configuration;
        //public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }
        public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<Login> Login { get; set; } = null;
		public DbSet<Register> Register { get; set; } = null;
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); optionsBuilder.UseSqlServer(connectionString);
        }
    }




}
