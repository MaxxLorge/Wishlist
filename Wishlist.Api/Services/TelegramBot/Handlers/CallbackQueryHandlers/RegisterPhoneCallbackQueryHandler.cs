using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class RegisterPhoneCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;

    public RegisterPhoneCallbackQueryHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public Func<string, bool> CallbackDataPredicate => s => s == CallbackQueries.RegisterPhone;

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