using Microsoft.AspNetCore.Mvc;

using Telegram.Bot.Types;

using Wishlist.Api.Filters;
using Wishlist.Api.Services;

namespace Wishlist.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdate(update, cancellationToken);
        return Ok();
    }
}