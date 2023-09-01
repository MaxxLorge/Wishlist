namespace Wishlist.Api.Services.Models.Errors;

public class StringError
{
    public StringError(string? error)
    {
        Error = error;
    }

    public string? Error { get; set; }
}