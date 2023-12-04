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
    public const string ShowMySubscribes = "showMySubscribes";

    public static string ShowDesireDetails(WishItem wishItem) => $"{Prefixes.ShowDesireDetailsPrefix}{Separator}{wishItem.Id}";

    public static string ShowUserContextMenu(User user) => $"{Prefixes.ShowUserContextMenuPrefix}{Separator}{user.Id}";
    
    public static string ShowUserDesires(User user) => $"{Prefixes.ShowUserDesiresPrefix}{Separator}{user.Id}";

    public static string SubscribeToUser(User user) => $"{Prefixes.SubscribeToUserPrefix}{Separator}{user.Id}";

    public static string RemoveWishItem(WishItem wishItem) => $"{Prefixes.RemoveWishItemPrefix}{Separator}{wishItem.Id}";

    public static string ChangeWishItemPriority(WishItem wishItem)
    {
        return $"{Prefixes.ChangeWishItemPriority}{Separator}{wishItem.Id}";
    }

    public static class Prefixes
    {
        public const string ShowDesireDetailsPrefix = "showDesireDetails";
        public const string ShowUserContextMenuPrefix = "showUserContextMenu";
        public const string ShowUserDesiresPrefix = "showUserDesires";
        public const string SubscribeToUserPrefix = "subscribeToUser";
        public const string RemoveWishItemPrefix = "removeWishItem";
        public const string ChangeWishItemPriority = "changePriority";
    }
}