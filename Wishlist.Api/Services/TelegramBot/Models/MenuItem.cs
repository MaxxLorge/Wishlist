namespace Wishlist.Api.Services.TelegramBot.Models;

public class MenuItem
{
    public MenuItem(string name, string callbackQuery)
    {
        Name = name;
        CallbackQuery = callbackQuery;
    }

    public  string Name { get;  }
    
    public string CallbackQuery { get; }

    public static class Sets
    {
        public static IReadOnlyCollection<MenuItem> Main = new[]
        {
            new MenuItem("Поделиться своим номером", "callback:registerPhone"),
            new MenuItem("Найти друга", "callback:findFriend")
        };
    }
}