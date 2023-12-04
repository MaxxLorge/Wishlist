namespace Wishlist.Api.Services.TelegramBot.StageKeeper;

public interface IStageKeeper
{
    public object? AdditionalData { get; set; }
    
    void SetStage(long telegramUserId, Stage stage);

    Stage GetOrAddStage(long telegramUserId, Stage defaultStage);
}