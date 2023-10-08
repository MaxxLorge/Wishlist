using Wishlist.DAL.Entities;

namespace Wishlist.Api.Services.TelegramBot;

public static class CallbackQueries
{
    public const char Separator = ':';
    
    public const string FindFriend = "findFriend";
    public const string RegisterNo = "registerNo";
    public const string RegisterYes = "registerYes";
    public const string RegisterPhone = "registerPhone";
    public const string AddDesire = "addDesire";
    public const string ShowMyDesires = "showMyDesires";

    public const string ShowDesireDetailsPrefix = "showDesireDetails";

    public static string ShowDesireDetails(WishItem wishItem) => $"{ShowDesireDetailsPrefix}{Separator}{wishItem.Id}";
}