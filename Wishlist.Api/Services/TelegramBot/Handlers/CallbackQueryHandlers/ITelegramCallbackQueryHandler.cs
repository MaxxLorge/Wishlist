using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public interface ITelegramCallbackQueryHandler
{
    Func<string, bool> CallbackDataPredicate { get; }
    
    Task Handle(CallbackQuery callbackQuery, CancellationToken ct);
    
    Stage StageAfterHandling { get; }
}