using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.Extensions;
using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL;
using Wishlist.DAL.Entities;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class SubscribeToUserQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly WishlistDbContext _context;

    public SubscribeToUserQueryHandler(ITelegramBotClient telegramBotClient,
        WishlistDbContext context)
    {
        _telegramBotClient = telegramBotClient;
        _context = context;
    }
    
    public Func<string, bool> CallbackDataPredicate =>
        s => s.StartsWith(CallbackQueries.Prefixes.SubscribeToUserPrefix);
    
    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var chatId = callbackQuery.Message!.Chat.Id;
        var userId = callbackQuery.GetUserIdFromCallbackData();
        var initiatorTelegramUserId = callbackQuery.GetCallbackInitiatorTelegramUserId();

        var initiatorUser = await _context.Users
            .Include(x => x.SubscribeToUsers)
            .SingleAsync(x => x.TelegramUserId == initiatorTelegramUserId, ct);

        if (initiatorUser.SubscribeToUsers.Any(x => x.Id == userId))
        {
            await _telegramBotClient.SendTextMessageAsync(
                chatId,
                "Вы уже подписаны на пользователя",
                cancellationToken: ct);
            return;
        }

        _context.Subscribes.Add(new Subscribe() { SubscribeFromId = initiatorUser.Id, SubscribeToId = userId });
        await _context.SaveChangesAsync(ct);

        await _telegramBotClient.SendTextMessageAsync(
            chatId,
            "Вы успешно подписались на пользователя",
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.Default;
}