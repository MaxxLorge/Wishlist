using Kontur.Results;

using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL;
using Wishlist.DAL.Entities;

using User = Wishlist.DAL.Entities.User;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class AddDesireMessageHandler : ITelegramMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly WishlistDbContext _wishlistDbContext;

    private const int MESSAGE_PARTS_COUNT = 5;
    
    private bool _isSuccessful = false;

    public AddDesireMessageHandler(ITelegramBotClient telegramBotClient, WishlistDbContext wishlistDbContext)
    {
        _telegramBotClient = telegramBotClient;
        _wishlistDbContext = wishlistDbContext;
    }
    
    public Func<Message, bool> MessagePredicate => _ => true;
    public Func<Stage, bool> StagePredicate => stage => stage == Stage.AddDesire;
    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        var messageText = message.Text;
        if (messageText == null)
            return Result.Fail("Отсутствует текст сообщения");

        var messageParts = messageText
            .Split('\n')
            .Select(x => string.IsNullOrWhiteSpace(x) ? null : x)
            .ToArray();
        
        if (messageParts.Length != MESSAGE_PARTS_COUNT)
        {
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "Неверный формат ввода. Попробуйте еще раз",
                cancellationToken: ct);
            _isSuccessful = false;
            return Result.Succeed();
        }

        var wishItem = new WishItem()
        {
            Name = messageParts[0],
            DesirabilityDegree = (DesirabilityDegree)int.Parse(messageParts[1]),
            Link = messageParts[2],
            Description = messageParts[3],
            Cost = decimal.Parse(messageParts[4])
        };

        var user = await _wishlistDbContext
            .Users
            .Include(x => x.Subscribers)
            .SingleAsync(x => x.TelegramUserId == message.From!.Id, ct);
        user.WishItems.Add(wishItem);

        await _wishlistDbContext.SaveChangesAsync(ct);
        _isSuccessful = true;

        await _telegramBotClient.SendTextMessageAsync(
            message.Chat.Id,
            "Желание успешно добавлено",
            cancellationToken: ct);

        await NotifySubscribers(user, wishItem);
        
        return Result.Succeed();
    }

    public Stage StageAfterHandling => _isSuccessful ? Stage.Default : Stage.AddDesire;

    private async Task NotifySubscribers(User currentUser, WishItem wishItem)
    {
        var subscribers = currentUser.Subscribers;
        foreach (var subscriber in subscribers)
        {
            await _telegramBotClient.SendTextMessageAsync(
                subscriber.TelegramChatId!,
                $"{currentUser.Username} добавил(а) новое желание{Environment.NewLine}" +
                $"{wishItem}");
        }
    }
}