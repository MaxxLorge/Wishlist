using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Wishlist.DAL.Entities;

namespace Wishlist.DAL;

public class WishlistDbContext : DbContext
{
    public WishlistDbContext(DbContextOptions<WishlistDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; } = null!;
}