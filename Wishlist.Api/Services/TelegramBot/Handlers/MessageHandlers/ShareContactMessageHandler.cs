using Kontur.Results;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.Factories;
using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL.Repositories;

using User = Wishlist.DAL.Entities.User;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class ShareContactMessageHandler : ITelegramMessageHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _telegramBotClient;

    public ShareContactMessageHandler(IUserRepository userRepository,
        ITelegramBotClient telegramBotClient)
    {
        _userRepository = userRepository;
        _telegramBotClient = telegramBotClient;
    }

    public Func<Message, bool> MessagePredicate => message => message.Contact?.PhoneNumber != null;

    public Func<Stage, bool> StagePredicate => stage => stage == Stage.ShareContact;

    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        var telegramUser = message.From!;
        var user = await _userRepository.FindByTelegramUserId(telegramUser.Id, ct);

        if (user == null)
            return Result.Fail("Не удалось найти пользователя");

        var phone = message.Contact?.PhoneNumber;
        if(phone == null)
            return Result<string, User>.Fail("Отсутствует номер телефона");
        
        user.Phone = phone;
        await _userRepository.Update(user, ct);

        await _telegramBotClient.SendTextMessageAsync(
            message.Chat.Id,
            "Ваш номер успешно зарегистрирован",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: ct);

        await _telegramBotClient
            .SendTextMessageAsync(
                message.Chat.Id,
                "Главное меню",
                replyMarkup: InlineKeyboardMarkupFactory.CreateMainMenu(),
                cancellationToken: ct);
        
        return Result.Succeed();
    }

    public Stage StageAfterHandling => Stage.Default;
}