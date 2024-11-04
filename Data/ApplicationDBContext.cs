using CLDVPart3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CLDVPart3.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderRequest> OrderRequests { get; set; }

        public DbSet<Document> Documents { get; set; }

    }
}
