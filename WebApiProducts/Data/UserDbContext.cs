using Microsoft.EntityFrameworkCore;
using WebApiProducts.Models;

namespace WebApiProducts.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<UserModel> userModels { get; set; }
        public DbSet<ProductModel> productModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>().ToTable("tbl_product");
            modelBuilder.Entity<UserModel>().ToTable("tbl_user");
        }
    }
}
