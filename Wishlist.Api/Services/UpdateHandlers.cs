using System.Text.RegularExpressions;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;
using Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;
using Wishlist.Api.Services.TelegramBot.StageKeeper;

namespace Wishlist.Api.Services;

public class UpdateHandlers
{
    private readonly IEnumerable<ITelegramMessageHandler> _telegramMessageHandlers;
    private readonly IEnumerable<ITelegramCallbackQueryHandler> _callbackQueryHandlers;
    private readonly IStageKeeper _stageKeeper;

    public UpdateHandlers(
        IEnumerable<ITelegramMessageHandler> telegramMessageHandlers,
        IEnumerable<ITelegramCallbackQueryHandler> callbackQueryHandlers,
        IStageKeeper stageKeeper)
    {
        _telegramMessageHandlers = telegramMessageHandlers;
        _callbackQueryHandlers = callbackQueryHandlers;
        _stageKeeper = stageKeeper;
    }
    
    public async Task HandleUpdate(Update update, CancellationToken ct)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleMessage(update, ct);
                break;
            case UpdateType.Unknown:
                break;
            case UpdateType.InlineQuery:
                break;
            case UpdateType.ChosenInlineResult:
                break;
            case UpdateType.CallbackQuery:
                await HandleCallbackQuery(update, ct);
                break;
            case UpdateType.EditedMessage:
                break;
            case UpdateType.ChannelPost:
                break;
            case UpdateType.EditedChannelPost:
                break;
            case UpdateType.ShippingQuery:
                break;
            case UpdateType.PreCheckoutQuery:
                break;
            case UpdateType.Poll:
                break;
            case UpdateType.PollAnswer:
                break;
            case UpdateType.MyChatMember:
                break;
            case UpdateType.ChatMember:
                break;
            case UpdateType.ChatJoinRequest:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task HandleMessage(Update update, CancellationToken ct)
    {
        var message = update.Message 
                      ?? throw new InvalidOperationException("Сообщение отсутствует");

        var stage = _stageKeeper.GetOrAddStage(update.Message.From!.Id, Stage.Default);
        
        var handler = _telegramMessageHandlers
            .SingleOrDefault(x => 
                x.StagePredicate(stage) &&
                x.MessagePredicate(message));
        if (handler == null)
            return;

        await handler.Handle(message, ct);

        _stageKeeper.SetStage(update.Message.From.Id, handler.StageAfterHandling);
    }

    private async Task HandleCallbackQuery(Update update, CancellationToken ct)
    {
        var callback = update.CallbackQuery
            ?? throw new InvalidOperationException("Отсутствует CallbackQuery");

        var handler = _callbackQueryHandlers
            .SingleOrDefault(x => x.CallbackDataPredicate(callback.Data!));
        
        if(handler == null)
            return;

        await handler.Handle(callback, ct);
        
        _stageKeeper.SetStage(update.CallbackQuery.From.Id, handler.StageAfterHandling);
    }
}