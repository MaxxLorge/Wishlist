using System.Text.RegularExpressions;

using Kontur.Results;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.Api.Services.Users;

using User = Wishlist.DAL.Entities.User;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class FindFriendMessageHandler : ITelegramMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUserService _userService;

    public FindFriendMessageHandler(ITelegramBotClient telegramBotClient,
        IUserService userService)
    {
        _telegramBotClient = telegramBotClient;
        _userService = userService;
    }

    public Func<Message, bool> MessagePredicate => _ => true;
    public Func<Stage, bool> StagePredicate => stage => stage == Stage.FindingFriend;

    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        var loginSubstring = message.Text;
        var users = await _userService.FindUsers(loginSubstring!, ct);

        if (users.Count > 0)
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "Результаты поиска",
                replyMarkup: CreateMarkup(users),
                cancellationToken: ct);
        else
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "Ничего не найдено",
                cancellationToken: ct);
        
        return Result<string>.Succeed();
    }

    public Stage StageAfterHandling => Stage.Default;

    private static InlineKeyboardMarkup CreateMarkup(IReadOnlyCollection<User> users) =>
        new(
            users.Select(x =>
                InlineKeyboardButton
                    .WithCallbackData(x.Username!, CallbackQueries.ShowUserContextMenu(x))));
}