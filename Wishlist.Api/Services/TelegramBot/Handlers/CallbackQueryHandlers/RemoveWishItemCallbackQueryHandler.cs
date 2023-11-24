using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.Extensions;
using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL.Repositories;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class RemoveWishItemCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly IWishItemRepository _wishItemRepository;
    private readonly ITelegramBotClient _telegramBotClient;

    public RemoveWishItemCallbackQueryHandler(IWishItemRepository wishItemRepository,
        ITelegramBotClient telegramBotClient)
    {
        _wishItemRepository = wishItemRepository;
        _telegramBotClient = telegramBotClient;
    }
    
    public Func<string, bool> CallbackDataPredicate => s => s.StartsWith(CallbackQueries.Prefixes.RemoveWishItemPrefix);
    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var wishItemId = callbackQuery.GetIdFromCallbackData();
        await _wishItemRepository.DeleteById(wishItemId, ct);

        await _telegramBotClient
            .SendTextMessageAsync(
                callbackQuery.GetCallbackInitiatorTelegramUserId(),
                "Желание удалено",
                cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.Default;
}