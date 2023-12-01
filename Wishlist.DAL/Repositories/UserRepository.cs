using Microsoft.EntityFrameworkCore;

using Wishlist.DAL.Entities;

namespace Wishlist.DAL.Repositories;

[Flags]
public enum UserIncludeOptions
{
    None = 0,
    Role = 1
}

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetAllUsers(CancellationToken ct);
    
    Task<User?> FindById(int id, CancellationToken ct);

    Task<User?> AddUser(User user, CancellationToken ct);

    Task<User?> FindByTelegramUserId(long telegramUserId, CancellationToken ct,
        UserIncludeOptions includeOptions = UserIncludeOptions.None);

    Task Update(User user, CancellationToken ct);

    Task<IReadOnlyCollection<User>> GetSubscribesByTelegramUserId(long telegramUserId, CancellationToken ct);

    Task<IReadOnlyCollection<User>> FindUsersByUsername(string loginSubstring, CancellationToken ct);
}

public class UserRepository : IUserRepository
{
    private readonly WishlistDbContext _context;

    public UserRepository(WishlistDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<User>> GetAllUsers(CancellationToken ct)
    {
        return await _context.Users.ToArrayAsync(ct);
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

    public Task<User?> FindByTelegramUserId(long telegramUserId, CancellationToken ct,
        UserIncludeOptions userIncludeOptions = UserIncludeOptions.None)
    {
        var query = _context.Users.AsQueryable();

        if (userIncludeOptions.HasFlag(UserIncludeOptions.Role))
            query = query.Include(x => x.Role);

        return query.SingleOrDefaultAsync(x => x.TelegramUserId == telegramUserId, ct);
    }

    public async Task Update(User user, CancellationToken ct)
    {
        _context.Update(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyCollection<User>> GetSubscribesByTelegramUserId(long telegramUserId,
        CancellationToken ct)
    {
        return await _context
            .Users
            .Where(x => x.TelegramUserId == telegramUserId)
            .SelectMany(x => x.SubscribeTo)
            .ToArrayAsync(ct);
    }

    public async Task<IReadOnlyCollection<User>> FindUsersByUsername(string loginSubstring, CancellationToken ct)
    {
        return await _context.Users
            .Where(x => x.Username!.Contains(loginSubstring))
            .ToArrayAsync(ct);
    }
}