using LightInject;

using Telegram.Bot;

using Wishlist.DAL;

namespace Wishlist.Api;

public class CompositionRoot : ICompositionRoot
{
    public void Compose(IServiceRegistry serviceRegistry)
    {
        var assembliesToRegister = new[]
        {
            typeof(Program).Assembly,
            typeof(WishlistDbContext).Assembly
        };

        foreach (var assembly in assembliesToRegister)
            serviceRegistry.RegisterAssembly(
                assembly, () => new PerScopeLifetime(),
                (serviceType, implementingType) => serviceType != typeof(IHostedService));

        serviceRegistry.RegisterSingleton<ITelegramBotClient, TelegramBotClient>();
    }
}