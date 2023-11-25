using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Wishlist.DAL.Entities;

namespace Wishlist.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(x => x.WishItems)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.SubscribeFrom)
            .WithMany(x => x.SubscribeTo)
            .UsingEntity<Subscribe>(
                l => l.HasOne<User>().WithMany().HasForeignKey(x => x.SubscribeToId),
                r => r.HasOne<User>().WithMany().HasForeignKey(x => x.SubscribeFromId));
    }
}