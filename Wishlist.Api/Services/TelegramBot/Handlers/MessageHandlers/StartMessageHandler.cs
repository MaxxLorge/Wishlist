using Kontur.Results;

using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.Factories;
using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.Api.Services.Users;
using Wishlist.Api.Settings;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class StartMessageHandler : ITelegramMessageHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IOptions<MessageTextsConfiguration> _messageTextConfig;
    private readonly IUserService _userService;

    public StartMessageHandler(ITelegramBotClient botClient,
        IOptions<MessageTextsConfiguration> messageTextConfig,
        IUserService userService)
    {
        _botClient = botClient;
        _messageTextConfig = messageTextConfig;
        _userService = userService;
    }

    public Func<Message, bool> MessagePredicate => message => message.Text == "/start";
    public Func<Stage, bool> StagePredicate => _ => true;

    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        await _botClient.SendTextMessageAsync(
            message.Chat.Id,
            _messageTextConfig.Value.Greeting,
            cancellationToken: ct);

        var telegramUser = message.From!;
        
        var user = await _userService.FindByTelegramUserId(telegramUser.Id, ct) 
                   ?? await _userService.AddPrimaryUserInfo(telegramUser.Username, telegramUser.FirstName, telegramUser.Id, message.Chat.Id, ct);

        if (user.Phone == null)
            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Вы не зарегистрированы. Хотите зарегистрироваться? Это даст возможность делиться своим списком желаний с друзьями.",
                replyMarkup: new InlineKeyboardMarkup(
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Да", CallbackQueries.RegisterYes),
                        InlineKeyboardButton.WithCallbackData("Нет", CallbackQueries.RegisterNo),
                    }),
                cancellationToken: ct);
        else
            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Главное меню",
                replyMarkup: InlineKeyboardMarkupFactory.CreateMainMenu(),
                cancellationToken: ct);

        return Result.Succeed();
    }

    public Stage StageAfterHandling => Stage.Default;
}