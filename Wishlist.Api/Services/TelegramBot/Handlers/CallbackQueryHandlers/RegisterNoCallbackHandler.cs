using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.Factories;
using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class RegisterNoCallbackHandler : ITelegramCallbackQueryHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IInlineKeyboardMarkupFactory _inlineKeyboardMarkupFactory;

    public RegisterNoCallbackHandler(ITelegramBotClient telegramBotClient,
        IInlineKeyboardMarkupFactory inlineKeyboardMarkupFactory)
    {
        _telegramBotClient = telegramBotClient;
        _inlineKeyboardMarkupFactory = inlineKeyboardMarkupFactory;
    }

    public string CallbackData => CallbackQueries.RegisterNo;

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            "Хорошо. Вы можете поделиться своим номером в любой момент через главное меню.",
            replyMarkup: _inlineKeyboardMarkupFactory.CreateMenu(),
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.Default;
}