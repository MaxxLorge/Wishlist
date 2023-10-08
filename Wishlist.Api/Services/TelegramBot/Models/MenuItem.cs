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
        public static readonly IReadOnlyCollection<MenuItem> Main = new[]
        {
            new MenuItem("Поделиться своим номером", CallbackQueries.RegisterPhone),
            new MenuItem("Найти друга", CallbackQueries.FindFriend),
            new MenuItem("Добавить желание", CallbackQueries.AddDesire),
            new MenuItem("Показать мой список желаний", CallbackQueries.ShowMyDesires),
        };
    }
}