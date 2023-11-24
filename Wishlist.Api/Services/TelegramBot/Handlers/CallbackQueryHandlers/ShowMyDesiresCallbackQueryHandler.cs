using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL;
using Wishlist.DAL.Entities;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class ShowMyDesiresCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly WishlistDbContext _context;
    private readonly ITelegramBotClient _telegramBotClient;

    public ShowMyDesiresCallbackQueryHandler(WishlistDbContext context, ITelegramBotClient telegramBotClient)
    {
        _context = context;
        _telegramBotClient = telegramBotClient;
    }

    public Func<string, bool> CallbackDataPredicate => s => s == CallbackQueries.ShowMyDesires;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var telegramUserId = callbackQuery.From.Id;

        var user = await _context
            .Users
            .Include(x => x.WishItems)
            .SingleAsync(x => x.TelegramUserId == telegramUserId, ct);
        var wishItems = user.WishItems;

        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            wishItems.Count > 0 ? "Ваш список желаний" : "Список пуст",
            replyMarkup: wishItems.Count > 0 ? CreateReplyMarkup(wishItems) : null,
            cancellationToken: ct);

    }

    public Stage StageAfterHandling => Stage.Default;

    private IReplyMarkup CreateReplyMarkup(ICollection<WishItem> wishItems) =>
        new InlineKeyboardMarkup(
            wishItems.Select(x => new []
            {
                InlineKeyboardButton.WithCallbackData(x.Name, CallbackQueries.ShowDesireDetails(x))
            })
        );
}