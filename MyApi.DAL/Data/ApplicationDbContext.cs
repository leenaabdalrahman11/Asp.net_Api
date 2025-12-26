using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApi.DAL.Models;

namespace MyApi.DAL.Data
{
    public sealed class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
        }
        
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                        .ToTable("Users");

            modelBuilder.Entity<IdentityRole>()
                        .ToTable("Roles");

            modelBuilder.Entity<IdentityUserRole<string>>()
                        .ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserClaim<string>>()
                        .ToTable("UserClaims");

            modelBuilder.Entity<IdentityUserLogin<string>>()
                        .ToTable("UserLogins");

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                        .ToTable("RoleClaims");

            modelBuilder.Entity<IdentityUserToken<string>>()
                        .ToTable("UserTokens");
        }
    }
}
