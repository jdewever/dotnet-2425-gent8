using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.DomainClasses;

namespace Rise.Persistence.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();

            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(e => e.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}