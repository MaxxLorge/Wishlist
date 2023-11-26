using Microsoft.EntityFrameworkCore;

using Wishlist.DAL.Configurations;
using Wishlist.DAL.Entities;

namespace Wishlist.DAL;

public class WishlistDbContext : DbContext
{
    public WishlistDbContext(DbContextOptions<WishlistDbContext> options) : base(options)
    {
        
    }

    public required DbSet<User> Users { get; set; }

    public required DbSet<WishItem> WishItems { get; set; }

    public required DbSet<Subscribe> Subscribes { get; set; }

    public required DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new WishItemConfiguration());
    }
}