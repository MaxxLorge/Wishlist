using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.Extensions;
using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL.Entities;
using Wishlist.DAL.Repositories;

using User = Wishlist.DAL.Entities.User;

namespace Wishlist.Api.Services.TelegramBot.Handlers.CallbackQueryHandlers;

public class ShowWishItemDetailsCallbackQueryHandler : ITelegramCallbackQueryHandler
{
    private readonly IWishItemRepository _wishItemRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _telegramBotClient;

    public ShowWishItemDetailsCallbackQueryHandler(
        IWishItemRepository wishItemRepository,
        IUserRepository userRepository,
        ITelegramBotClient telegramBotClient)
    {
        _wishItemRepository = wishItemRepository;
        _userRepository = userRepository;
        _telegramBotClient = telegramBotClient;
    }
    
    public Func<string, bool> CallbackDataPredicate => s => s.StartsWith(CallbackQueries.Prefixes.ShowDesireDetailsPrefix);

    public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
    {
        var wishItemId = callbackQuery.GetIdFromCallbackData();

        var wishItem = await _wishItemRepository
            .FindById(wishItemId, ct);
        var user = await _userRepository
            .FindByTelegramUserId(callbackQuery.GetCallbackInitiatorTelegramUserId(), ct);

        await _telegramBotClient.SendTextMessageAsync(
            callbackQuery.Message!.Chat.Id,
            CreateDetailsString(wishItem!),
            replyMarkup: CreateReplyMarkup(wishItem!, user!),
            cancellationToken: ct);
    }

    public Stage StageAfterHandling => Stage.Default;

    private static string CreateDetailsString(WishItem wishItem) => wishItem.ToString();

    private static IReplyMarkup? CreateReplyMarkup(WishItem wishItem, User user)
    {
        if (user.Id != wishItem.UserId)
            return null;

        return new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Изменить приоритет",
                CallbackQueries.ChangeWishItemPriority(wishItem)),
            InlineKeyboardButton.WithCallbackData("Удалить", CallbackQueries.RemoveWishItem(wishItem))
        });
    }
}