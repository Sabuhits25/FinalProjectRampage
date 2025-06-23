using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rampage.Core.Entities;
using Rampage.Core.Entities.Commons;
using Rampage.Core.Entities.Identity;
using Rampage.Core.Models;
using Rampage.DAL.Services.Interface;
using System.Reflection;


namespace Rampage.DAL.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {

        private readonly IClaimService _claimService;

        public AppDbContext(DbContextOptions<AppDbContext> options, IClaimService claimService) : base(options)
        {
            _claimService = claimService;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTranslation> BlogTranslations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ColorTranslation> ColorTranslations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSetting> ProductSettings { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _claimService.GetUserId() ?? "Unkown User";
                        entry.Entity.CreatedOn = DateTime.UtcNow;

                        entry.Entity.UpdatedBy = _claimService.GetUserId() ?? "Unkown User";
                        entry.Entity.UpdatedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:

                        if (entry.Entity.CreatedBy is null)
                        {
                            entry.Entity.CreatedBy = _claimService.GetUserId() ?? "Unkown User";
                            entry.Entity.CreatedOn = DateTime.UtcNow;
                        }

                        entry.Entity.UpdatedBy = _claimService.GetUserId() ?? "Unkown User";
                        entry.Entity.UpdatedOn = DateTime.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

