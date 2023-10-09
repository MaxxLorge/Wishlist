namespace Wishlist.DAL.Entities;

public class Subscribe
{
    public int Id { get; set; }

    public int SubscribeFromId { get; set; }
    
    public int SubscribeToId { get; set; }
}