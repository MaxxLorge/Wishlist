namespace Wishlist.Api.Settings;

public class MessageTextsConfiguration
{
    public const string ConfigurationName = nameof(MessageTextsConfiguration);
    
    public required string Greeting { get; set; }
}