using System.ComponentModel.DataAnnotations;

namespace Wishlist.DAL.Entities;

public class User
{
    public int Id { get; init; }

    public required string? Username { get; set; } 

    public required string Name { get; set; }

    public long? TelegramUserId { get; set; }

    [Phone]
    public string? Phone { get; set; }
}