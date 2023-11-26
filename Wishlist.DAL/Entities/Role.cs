namespace Wishlist.DAL.Entities;

public enum RoleType : byte
{
    User,
    Admin
}

public class Role
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public required User User { get; set; }

    public RoleType RoleType { get; set; }
}