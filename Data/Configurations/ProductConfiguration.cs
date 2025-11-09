using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Url)
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(p => p.Price);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.StockQuantity)
                .HasDefaultValue(0);
        }
    }
}
