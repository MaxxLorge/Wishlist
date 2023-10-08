using Microsoft.EntityFrameworkCore;

using Wishlist.DAL.Configurations;
using Wishlist.DAL.Entities;

namespace Wishlist.DAL;

public class WishlistDbContext : DbContext
{
    public WishlistDbContext(DbContextOptions<WishlistDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }

    public DbSet<WishItem> WishItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new WishItemConfiguration());
        
    }
}