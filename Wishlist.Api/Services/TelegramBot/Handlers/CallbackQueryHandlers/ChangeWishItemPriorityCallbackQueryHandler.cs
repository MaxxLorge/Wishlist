using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.Extensions;
using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class ChangeWishItemPriorityCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IStageKeeper _stageKeeper;

    public ChangeWishItemPriorityCallbackQueryHandler(ITelegramBotClient telegramBotClient,
        IStageKeeper stageKeeper)
    {
        _telegramBotClient = telegramBotClient;
        _stageKeeper = stageKeeper;
    }

    public Func<string, bool> CallbackDataPredicate => _ => true;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            "Введите число от 1 до 10",
            cancellationToken: ct);
        _stageKeeper.AdditionalData = callbackQuery.GetIdFromCallbackData();
    }

    public Stage StageAfterHandling => Stage.ChangingWishItemPriority;
}