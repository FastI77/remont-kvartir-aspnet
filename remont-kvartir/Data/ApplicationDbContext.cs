using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using remont_kvartir.Models;

namespace remont_kvartir.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RepairRequest> RepairRequests { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<PortfolioItem> PortfolioItems { get; set; } = null!;
        public DbSet<PortfolioImage> PortfolioImages { get; set; } = null!;

        public DbSet<Review> Reviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RepairRequest>()
                .HasOne(r => r.User)
                .WithMany(u => u.RepairRequests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}