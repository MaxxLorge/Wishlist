using Wishlist.DAL.Entities;

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

        public static IReadOnlyCollection<MenuItem> UserContextMenu(User user)
        {
            return new[]
            {
                new MenuItem("Список желаний", CallbackQueries.ShowUserDesires(user)),
                new MenuItem("Подписаться", CallbackQueries.SubscribeToUser(user))
            };
        }
    }
}