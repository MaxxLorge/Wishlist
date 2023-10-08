namespace Wishlist.Api.Services.TelegramBot.StageKeeper;

public interface IStageKeeper
{
    void SetStage(long telegramUserId, Stage stage);

    Stage GetOrAddStage(long telegramUserId, Stage defaultStage);
}