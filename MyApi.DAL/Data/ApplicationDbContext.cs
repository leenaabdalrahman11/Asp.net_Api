using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApi.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MyApi.DAL.Data
{
    public sealed class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    IHttpContextAccessor? httpContextAccessor = null)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
             var entries = ChangeTracker.Entries<BaseModel>();
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseModel>();
            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;

                }
            }


            return base.SaveChanges();
        }
    }
}
