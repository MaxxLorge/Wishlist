namespace Wishlist.DAL.Entities;

public enum RoleType : byte
{
    User,
    Admin
}

public class Role
{
    public Role(RoleType roleType)
    {
        RoleType = roleType;
    }

    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public RoleType RoleType { get; set; }
}