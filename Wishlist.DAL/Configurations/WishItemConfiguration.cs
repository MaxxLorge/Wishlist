using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Wishlist.DAL.Entities;

namespace Wishlist.DAL.Configurations;

public class WishItemConfiguration : IEntityTypeConfiguration<WishItem>
{
    public void Configure(EntityTypeBuilder<WishItem> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.WishItems)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(x => x.Description)
            .HasMaxLength(ConstraintConstants.DefaultTextLength);

        builder
            .Property(x => x.Name)
            .HasMaxLength(ConstraintConstants.DefaultNameLength);
    }
}