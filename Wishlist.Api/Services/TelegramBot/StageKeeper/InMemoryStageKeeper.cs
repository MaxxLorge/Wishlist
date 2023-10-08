using System.Collections.Concurrent;

namespace Wishlist.Api.Services.TelegramBot.StageKeeper;

public class InMemoryStageKeeper : IStageKeeper
{
    private static readonly ConcurrentDictionary<long, Stage> Stages = new();
    
    public Stage Stage { get; private set; } = Stage.Default;

    public void SetStage(long telegramUserId, Stage stage) =>
        Stages
            .AddOrUpdate(telegramUserId, id => stage, (_, _) => stage);

    public Stage GetOrAddStage(long telegramUserId, Stage defaultStage = Stage.Default) =>
        Stages.GetOrAdd(telegramUserId, defaultStage);
}