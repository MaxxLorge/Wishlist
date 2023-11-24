using Microsoft.EntityFrameworkCore;

using Wishlist.DAL.Entities;

namespace Wishlist.DAL.Repositories;

public interface IWishItemRepository
{
    Task<WishItem?> FindById(int id, CancellationToken ct);
    Task<WishItem> Add(WishItem wishItem, CancellationToken ct);
    Task DeleteById(int wishItemId, CancellationToken ct);
}

public class WishItemRepository : IWishItemRepository
{
    private readonly WishlistDbContext _context;

    public WishItemRepository(WishlistDbContext context)
    {
        _context = context;
    }

    public async Task<WishItem?> FindById(int id, CancellationToken ct)
    {
        return await _context
            .WishItems
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<WishItem> Add(WishItem wishItem, CancellationToken ct)
    {
        _context.Add(wishItem);
        await _context.SaveChangesAsync(ct);
        return wishItem;
    }

    public async Task DeleteById(int wishItemId, CancellationToken ct)
    {
        var wishItem = await _context
            .WishItems
            .SingleOrDefaultAsync(x => x.Id == wishItemId, ct);
        
        //TODO: обработать null
        if(wishItem is null)
            return;

        _context.WishItems
            .Remove(wishItem);
        await _context
            .SaveChangesAsync(ct);
    }
}