using System.Collections.Concurrent;

namespace Wishlist.Api.Services;

[Flags]
public enum Stage
{
    Default = 0,
    FindingFriend = 1,
    ShareContact = 2,
}

public interface IStageKeeper
{
    void SetStage(long telegramUserId, Stage stage);

    Stage GetOrAddStage(long telegramUserId, Stage defaultStage);
}

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