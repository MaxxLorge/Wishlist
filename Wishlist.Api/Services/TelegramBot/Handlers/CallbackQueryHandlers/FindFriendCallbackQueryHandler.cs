using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class FindFriendCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly IStageKeeper _stageKeeper;
    private readonly ITelegramBotClient _telegramBotClient;

    public FindFriendCallbackQueryHandler(IStageKeeper stageKeeper,
        ITelegramBotClient telegramBotClient)
    {
        _stageKeeper = stageKeeper;
        _telegramBotClient = telegramBotClient;
    }

    public Func<string, bool> CallbackDataPredicate => s => s == CallbackQueries.FindFriend;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var user = callbackQuery.From;
        _stageKeeper.SetStage(user.Id, Stage.FindingFriend);

        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            "Введите никнейм",
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.FindingFriend;
}