using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class RegisterYesCallbackHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;

    public RegisterYesCallbackHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public Func<string, bool> CallbackDataPredicate => s => s == CallbackQueries.RegisterYes;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        await _telegramBotClient
            .SendTextMessageAsync(
                callbackQuery.Message!.Chat.Id,
                "Поделитесь телефоном",
                replyMarkup: new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact("Поделиться контактом")),
                cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.ShareContact;
}