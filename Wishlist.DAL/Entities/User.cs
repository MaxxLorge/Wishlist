using System.ComponentModel.DataAnnotations;

using Wishlist.DAL.Extensions;

namespace Wishlist.DAL.Entities;

public class User
{
    public int Id { get; init; }

    [Display(Name = "Никнейм")]
    public string? Username { get; set; } 

    [Display(Name = "Имя")]
    public string Name { get; set; } = null!;

    public long? TelegramUserId { get; set; }

    public long? TelegramChatId { get; set; }

    [Phone]
    [Display(Name = "Номер телефона")]
    public string? Phone { get; set; }

    public Role Role { get; set; } = null!;

    public ICollection<WishItem> WishItems { get; set; } = new List<WishItem>();

    public ICollection<User> SubscribeTo { get; set; } = new List<User>();

    public ICollection<User> SubscribeFrom { get; set; } = new List<User>();

    public override string ToString() => this.DisplaySeparatedByNewLines();
}