namespace Wishlist.Api.Settings;

public class BotConfiguration
{
    public const string ConfigurationName = nameof(BotConfiguration);

    public required string BotToken { get; init; }
    public required string HostAddress { get; init; }
    public required string Route { get; init; }
    public required string SecretToken { get; init; }
}