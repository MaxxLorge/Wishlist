using Telegram.Bot.Types;

namespace Wishlist.Api.Services.TelegramBot.Extensions;

public static class CallbackDataExtensions
{
    public static int GetUserIdFromCallbackData(this CallbackQuery callbackQuery)
    {
        if (callbackQuery.Data == null)
            throw new InvalidOperationException(
                $"В {nameof(CallbackQuery)} отстуствует {nameof(CallbackQuery.Data)}");
        
        return int.Parse(callbackQuery.Data.Split(CallbackQueries.Separator)[1]);
    }

    public static long GetCallbackInitiatorTelegramUserId(this CallbackQuery callbackQuery) => callbackQuery.From.Id;
}