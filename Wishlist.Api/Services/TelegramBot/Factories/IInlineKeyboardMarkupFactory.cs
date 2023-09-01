using Telegram.Bot.Types.ReplyMarkups;

using Wishlist.Api.Services.TelegramBot.Models;

namespace Wishlist.Api.Services.TelegramBot.Factories;

public interface IInlineKeyboardMarkupFactory
{
    InlineKeyboardMarkup CreateMenu();
}

public class InlineKeyboardMarkupFactory : IInlineKeyboardMarkupFactory
{
    public InlineKeyboardMarkup CreateMenu() =>
        new(
            MenuItem.Sets.Main
                .Select(item => 
                    new []{ InlineKeyboardButton.WithCallbackData(item.Name, item.CallbackQuery) }));
}