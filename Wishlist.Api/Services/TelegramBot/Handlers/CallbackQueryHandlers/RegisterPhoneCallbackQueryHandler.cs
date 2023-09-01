using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class RegisterPhoneCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;

    public RegisterPhoneCallbackQueryHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    
    public string CallbackData => "callback:registerPhone";
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