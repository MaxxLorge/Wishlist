using Kontur.Results;

using Telegram.Bot.Types;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public interface ITelegramMessageHandler
{
    public Func<Message, bool> MessagePredicate { get; }

    public Func<Stage, bool> StagePredicate { get; }
    
    Task<Result<string>> Handle(Message message, CancellationToken ct);
    
    public Stage StageAfterHandling { get; }
}