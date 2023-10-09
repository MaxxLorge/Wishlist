using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.Models;
using Wishlist.DAL.Entities;

namespace Wishlist.Api.Services.TelegramBot.Factories;

public static class InlineKeyboardMarkupFactory 
{
    public static InlineKeyboardMarkup CreateMainMenu() =>
        CreateMenu(MenuItem.Sets.Main);

    public static InlineKeyboardMarkup CreateUserContextMenu(User user) =>
        CreateMenu(MenuItem.Sets.UserContextMenu(user));

    private static InlineKeyboardMarkup CreateMenu(IReadOnlyCollection<MenuItem> menuItems) =>
        new(menuItems
            .Select(item => 
                new [] {InlineKeyboardButton.WithCallbackData(item.Name, item.CallbackQuery)}));
}