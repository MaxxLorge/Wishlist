using Kontur.Results;

using Telegram.Bot;
using Telegram.Bot.Types;

using Wishlist.Api.Services.TelegramBot.StageKeeper;
using Wishlist.DAL.Entities;
using Wishlist.DAL.Repositories;

namespace Wishlist.Api.Services.TelegramBot.Handlers.MessageHandlers;

public class ChangeWishItemPriorityMessageHandler : ITelegramMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IStageKeeper _stageKeeper;
    private readonly IWishItemRepository _wishItemRepository;

    public ChangeWishItemPriorityMessageHandler(ITelegramBotClient telegramBotClient,
        IStageKeeper stageKeeper,
        IWishItemRepository wishItemRepository)
    {
        _telegramBotClient = telegramBotClient;
        _stageKeeper = stageKeeper;
        _wishItemRepository = wishItemRepository;
    }

    public Func<Message, bool> MessagePredicate => _ => true;
    public Func<Stage, bool> StagePredicate => stage => stage == Stage.ChangingWishItemPriority;

    public async Task<Result<string>> Handle(Message message, CancellationToken ct)
    {
        if (!int.TryParse(message.Text?.Trim(), out var priority))
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "Не удалось распознать ввод",
                cancellationToken: ct);

        if (priority is <= 0 or > 10)
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "Необходимо ввести число от 1 до 10",
                cancellationToken: ct);

        var wishItemId = (int)_stageKeeper.AdditionalData!;
        var wishItem = await _wishItemRepository.FindById(wishItemId, ct);

        wishItem!.DesirabilityDegree = (DesirabilityDegree)priority;

        await _telegramBotClient.SendTextMessageAsync(
            message.Chat.Id,
            "Приоритет был успешно изменен",
            cancellationToken: ct);

        return Result.Succeed();
    }

    public Stage StageAfterHandling => Stage.Default;
}