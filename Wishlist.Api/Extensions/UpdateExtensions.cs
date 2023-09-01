using Telegram.Bot.Types;

namespace Wishlist.Api.Extensions;

public static class UpdateExtensions
{
    public static long GetChatIdFromMessage(this Update update)
    {
        var message = update.Message
            ?? throw new InvalidOperationException("Сообщение отстутсвует");

        return message.Chat.Id;
    }
}