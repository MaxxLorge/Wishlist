using Microsoft.EntityFrameworkCore;

using Wishlist.DAL.Entities;

namespace Wishlist.DAL.Repositories;

public interface IUserRepository
{
    Task<User?> FindById(int id, CancellationToken ct);

    Task<User?> AddUser(User user, CancellationToken ct);

    Task<User?> FindByTelegramUserId(long telegramUserId, CancellationToken ct);

    Task Update(User user, CancellationToken ct);

    Task<IReadOnlyCollection<User>> FindUsersByUsername(string loginSubstring, CancellationToken ct);
}

public class UserRepository : IUserRepository
{
    private readonly WishlistDbContext _context;

    public UserRepository(WishlistDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindById(int id, CancellationToken ct) =>
        await _context
            .Users
            .SingleOrDefaultAsync(x => x.Id == id, ct);

    public async Task<User?> AddUser(User user, CancellationToken ct)
    {
        _context.Add(user);
        var savedCount = await _context.SaveChangesAsync(ct);

        return savedCount > 1 ? user : null;
    }

    public Task<User?> FindByTelegramUserId(long telegramUserId, CancellationToken ct) => 
        _context.Users.SingleOrDefaultAsync(x => x.TelegramUserId == telegramUserId, ct);

    public async Task Update(User user, CancellationToken ct)
    {
        _context.Update(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyCollection<User>> FindUsersByUsername(string loginSubstring, CancellationToken ct)
    {
        return await _context.Users
            .Where(x => x.Username!.Contains(loginSubstring))
            .ToArrayAsync(ct);
    }
}