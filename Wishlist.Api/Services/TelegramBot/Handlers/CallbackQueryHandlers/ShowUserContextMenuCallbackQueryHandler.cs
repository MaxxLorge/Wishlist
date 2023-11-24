using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.Extensions;
using Wishlist.Api.Services.TelegramBot.Factories;
using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class ShowUserContextMenuCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly WishlistDbContext _context;

    public ShowUserContextMenuCallbackQueryHandler(ITelegramBotClient telegramBotClient,
        WishlistDbContext context)
    {
        _telegramBotClient = telegramBotClient;
        _context = context;
    }

    public Func<string, bool> CallbackDataPredicate => s => s.StartsWith(CallbackQueries.Prefixes.ShowUserContextMenuPrefix);
    
    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var userId = callbackQuery.GetIdFromCallbackData();

        var user = await _context.Users.SingleAsync(x => x.Id == userId, ct);

        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            user.ToString(),
            replyMarkup: InlineKeyboardMarkupFactory.CreateUserContextMenu(user),
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.Default;
}