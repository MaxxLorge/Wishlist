using Kontur.Results;

using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.Factories;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class ShowMenuMessageHandler : ITelegramMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IInlineKeyboardMarkupFactory _inlineKeyboardMarkupFactory;

    public ShowMenuMessageHandler(ITelegramBotClient telegramBotClient,
        IInlineKeyboardMarkupFactory inlineKeyboardMarkupFactory)
    {
        _telegramBotClient = telegramBotClient;
        _inlineKeyboardMarkupFactory = inlineKeyboardMarkupFactory;
    }
    
    public Func<Message, bool> MessagePredicate => message => message.Text == "/menu";
    public Func<Stage, bool> StagePredicate => _ => true;
    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        await _telegramBotClient.SendTextMessageAsync(
            message.Chat.Id,
            "Главное меню",
            replyMarkup: _inlineKeyboardMarkupFactory.CreateMenu(),
            cancellationToken: ct);
        return Result.Succeed();
    }

    public Stage StageAfterHandling => Stage.Default;
}