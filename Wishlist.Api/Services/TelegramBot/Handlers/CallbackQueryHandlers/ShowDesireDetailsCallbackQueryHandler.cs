using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL;
using Wishlist.DAL.Entities;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class ShowDesireDetailsCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly WishlistDbContext _context;
    private readonly ITelegramBotClient _telegramBotClient;

    public ShowDesireDetailsCallbackQueryHandler(WishlistDbContext context,
        ITelegramBotClient telegramBotClient)
    {
        _context = context;
        _telegramBotClient = telegramBotClient;
    }
    
    public Func<string, bool> CallbackDataPredicate => s => s.StartsWith(CallbackQueries.Prefixes.ShowDesireDetailsPrefix);

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var wishItemId = int.Parse(callbackQuery.Data!.Split(CallbackQueries.Separator)[1]);

        var wishItem = await _context
            .WishItems
            .SingleAsync(x => x.Id == wishItemId, ct);

        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            CreateDetailsString(wishItem),
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.Default;

    private static string CreateDetailsString(WishItem wishItem) => wishItem.ToString();
}