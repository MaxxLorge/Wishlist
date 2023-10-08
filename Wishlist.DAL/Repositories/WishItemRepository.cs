using Wishlist.DAL.Entities;

namespace Wishlist.DAL.Repositories;

public interface IWishItemRepository
{
    Task<WishItem> Add(WishItem wishItem, CancellationToken ct);
}

public class WishItemRepository : IWishItemRepository
{
    private readonly WishlistDbContext _context;

    public WishItemRepository(WishlistDbContext context)
    {
        _context = context;
    }

    public async Task<WishItem> Add(WishItem wishItem, CancellationToken ct)
    {
        _context.Add(wishItem);
        await _context.SaveChangesAsync(ct);
        return wishItem;
    }
}