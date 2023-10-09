using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.Factories;
using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class RegisterNoCallbackHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;

    public RegisterNoCallbackHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public Func<string, bool> CallbackDataPredicate => s => s == CallbackQueries.RegisterNo;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            "Хорошо. Вы можете поделиться своим номером в любой момент через главное меню.",
            replyMarkup: InlineKeyboardMarkupFactory.CreateMainMenu(),
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.Default;
}