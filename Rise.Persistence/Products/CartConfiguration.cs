using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.DomainClasses;

namespace Rise.Persistence.Carts;

/// <summary>
/// Specific configuration for <see cref="Cart"/>.
/// </summary>
internal class CartConfiguration : EntityConfiguration<Cart>
{
    public override void Configure(EntityTypeBuilder<Cart> builder)
    {
        base.Configure(builder);
    }
}