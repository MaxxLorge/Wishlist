using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.Extensions;
using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL;
using Wishlist.DAL.Entities;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class ShowUserDesiresCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly WishlistDbContext _context;

    public ShowUserDesiresCallbackQueryHandler(ITelegramBotClient telegramBotClient,
        WishlistDbContext context)
    {
        _telegramBotClient = telegramBotClient;
        _context = context;
    }
    
    public Func<string, bool> CallbackDataPredicate => s => s.StartsWith(CallbackQueries.Prefixes.ShowUserDesiresPrefix);
    
    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var userId = callbackQuery.GetIdFromCallbackData();

        var user = await _context
            .Users
            .Include(x => x.WishItems)
            .SingleAsync(x => x.Id == userId, ct);
        var wishItems = user.WishItems;

        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            wishItems.Count > 0 ? "Список желаний" : "Список пуст",
            replyMarkup: wishItems.Count > 0 ? CreateReplyMarkup(wishItems) : null,
            cancellationToken: ct);

    }

    public Stage StageAfterHandling => Stage.Default;

    private IReplyMarkup CreateReplyMarkup(ICollection<WishItem> wishItems) =>
        new InlineKeyboardMarkup(
            wishItems.Select(x => new[]
            {
                InlineKeyboardButton.WithCallbackData(x.Name, CallbackQueries.ShowDesireDetails(x)),
            }));
}