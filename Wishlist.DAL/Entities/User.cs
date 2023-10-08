using System.ComponentModel.DataAnnotations;

namespace Wishlist.DAL.Entities;

public class User
{
    public int Id { get; init; }

    public string? Username { get; set; } 

    public string Name { get; set; } = null!;

    public long? TelegramUserId { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public ICollection<WishItem> WishItems { get; set; } = new List<WishItem>();
}