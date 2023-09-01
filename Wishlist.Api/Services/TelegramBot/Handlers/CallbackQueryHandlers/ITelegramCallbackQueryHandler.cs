using Telegram.Bot.Types;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public interface ITelegramCallbackQueryHandler
{
    string CallbackData { get; }
    
    Task Handle(CallbackQuery callbackQuery, CancellationToken ct);
    
    Stage StageAfterHandling { get; }
}