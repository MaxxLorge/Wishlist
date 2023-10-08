using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class AddDesireCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;

    public AddDesireCallbackQueryHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    
    public string CallbackData => CallbackQueries.AddDesire;
    
    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            $"<b>Введите данные в следующем формате (каждый параметр с новой строки, опциональные параметры можно не указывать, оставив пустую строку)</b>:{Environment.NewLine}" +
            $"<b>Название</b>{Environment.NewLine}" +
            $"<b>Степень желанности</b> - число от 1 до 10 (опционально){Environment.NewLine}" +
            $"<b>Ссылка</b> (опционально){Environment.NewLine}" +
            $"<b>Описание</b> (опционально){Environment.NewLine}" +
            $"<b>Стоимость</b> - число (опционально){Environment.NewLine}",
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.AddDesire;
}