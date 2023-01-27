using Microsoft.EntityFrameworkCore;
using Binned.Models;
using System.Collections.Generic;

namespace Binned
{
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public MyDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("MyConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Product> Products { get; set; }
    }
}