using Kontur.Results;

using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL.Entities;
using Wishlist.DAL.Extensions;
using Wishlist.DAL.Repositories;

using User = Wishlist.DAL.Entities.User;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class SendBroadcastMessageHandler : ITelegramMessageHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<SendBroadcastMessageHandler> _logger;

    public SendBroadcastMessageHandler(IUserRepository userRepository,
        ITelegramBotClient telegramBotClient,
        ILogger<SendBroadcastMessageHandler> logger)
    {
        _userRepository = userRepository;
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public Func<Message, bool> MessagePredicate => message => message.Text?.StartsWith("/sendInfoMessage") == true;
    public Func<Stage, bool> StagePredicate => _ => true;

    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        var telegramUserId = message.From!.Id;
        var user = await _userRepository.FindByTelegramUserId(telegramUserId, ct, UserIncludeOptions.Role)
                   ?? throw new InvalidOperationException();

        if (!IsAllowedUser(user))
        {
            _logger.LogWarning("Пользователь не имеет прав отпралять сообщения");
            return Result<string>.Fail("Пользователь не имеет прав отпралять сообщения");
        }

        var messageArgs = ParseToMessageArgs(message.Text!);
        _logger.LogInformation("{Message}", messageArgs.DisplaySeparatedByNewLines());

        if (!messageArgs.IsBroadcast)
            throw new NotImplementedException();

        var allUsers = await _userRepository.GetAllUsers(ct);

        await Parallel.ForEachAsync(allUsers, ct, async (u, token) =>
        {
            await _telegramBotClient
                .SendTextMessageAsync(
                    u.TelegramChatId!,
                    messageArgs.Text,
                    cancellationToken: token);
        });

        return Result.Succeed();
    }

    public Stage StageAfterHandling => Stage.Default;

    private static bool IsAllowedUser(User user)
    {
        return user.Role.RoleType == RoleType.Admin;
    }

    private static MessageArgs ParseToMessageArgs(string messageText)
    {
        var textSeparatedByNewLines = messageText.Split('\n');
        //TODO: реализовать отправку по userNames
        //var userNames = textSeparatedByNewLines[0].Split(',');
        var isBroadcast = textSeparatedByNewLines[1] == "true";
        var textMessage = string.Join('\n', textSeparatedByNewLines[2..]);

        if (!isBroadcast)
            throw new NotImplementedException();

        return new MessageArgs
        {
            IsBroadcast = isBroadcast,
            Text = textMessage
        };
    }

    private class MessageArgs
    {
        public string[] Usernames { get; set; } = Array.Empty<string>();

        public bool IsBroadcast { get; set; }

        public required string Text { get; set; }
    }
}