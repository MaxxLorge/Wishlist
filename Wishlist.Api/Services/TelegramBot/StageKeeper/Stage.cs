namespace Wishlist.Api.Services.TelegramBot.StageKeeper;

[Flags]
public enum Stage
{
    Default = 0,
    FindingFriend = 1,
    ShareContact = 2,
    AddDesire = 4
}