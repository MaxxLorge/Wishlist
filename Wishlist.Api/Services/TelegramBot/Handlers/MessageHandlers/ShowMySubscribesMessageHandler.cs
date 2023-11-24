using Kontur.Results;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL.Repositories;

using User = Wishlist.DAL.Entities.User;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class ShowMySubscribesMessageHandler : ITelegramMessageHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _telegramBotClient;

    public ShowMySubscribesMessageHandler(IUserRepository userRepository,
        ITelegramBotClient telegramBotClient)
    {
        _userRepository = userRepository;
        _telegramBotClient = telegramBotClient;
    }
    
    public Func<Message, bool> MessagePredicate => message => message.Text == "/subscribes";
    public Func<Stage, bool> StagePredicate => _ => true;
    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        var chatId = message.Chat.Id;
        var telegramUserId = message.From!.Id;

        var subscribes = await _userRepository
            .GetSubscribesByTelegramUserId(telegramUserId, ct);

        await _telegramBotClient.SendTextMessageAsync(
            chatId,
            subscribes.Count > 0 ? "Ваши подписки:" : "Нет подписок",
            replyMarkup: CreateReplyMarkup(subscribes),
            cancellationToken: ct);
        
        return Result.Succeed();
    }

    public Stage StageAfterHandling => Stage.Default;

    private static IReplyMarkup CreateReplyMarkup(IReadOnlyCollection<User> subscribes)
    {
        return new InlineKeyboardMarkup(
            subscribes.Select(x => new []
            {
                InlineKeyboardButton.WithCallbackData(x.Username!, CallbackQueries.ShowUserContextMenu(x)), 
            }));
    }
}