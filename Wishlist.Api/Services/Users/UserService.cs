using Wishlist.DAL.Entities;
using Wishlist.DAL.Repositories;

namespace Wishlist.Api.Services.Users;

public interface IUserService
{
    Task<User?> FindByTelegramUserId(long telegramUserId, CancellationToken ct);

    Task<User> AddPrimaryUserInfo(string? username,
        string firstName,
        long telegramUserId,
        long chatId,
        CancellationToken ct);

    Task<bool> UpdatePhone(int userId, string phone, CancellationToken ct);

    Task<IReadOnlyCollection<User>> FindUsers(string loginSubstring, CancellationToken ct);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> FindByTelegramUserId(long telegramUserId, CancellationToken ct) => 
        await _userRepository.FindByTelegramUserId(telegramUserId, ct);

    public async Task<User> AddPrimaryUserInfo(
        string? username,
        string firstName,
        long telegramUserId,
        long chatId,
        CancellationToken ct)
    {
        var userToAdd = new User()
        {
            Username = username,
            Name = firstName,
            TelegramUserId = telegramUserId,
            TelegramChatId = chatId,
            Role = new Role(RoleType.User)
        };

        await _userRepository
            .AddUser(userToAdd,
                ct);

        return userToAdd;
    }

    public async Task<bool> UpdatePhone(int userId, string phone, CancellationToken ct)
    {
        var user = await _userRepository.FindById(userId, ct);

        if (user == null)
            return false;

        user.Phone = phone;
        await _userRepository.Update(user, ct);
        return true;
    }

    public async Task<IReadOnlyCollection<User>> FindUsers(string loginSubstring, CancellationToken ct) =>
        await _userRepository
            .FindUsersByUsername(loginSubstring, ct);
}