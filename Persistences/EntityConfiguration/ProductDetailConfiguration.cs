using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Persistences.EntityConfiguration
{
    internal class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.ToTable("ProductDetails", "dbo");

            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.Width).HasColumnName("Width");
            builder.Property(x => x.Height).HasColumnName("Height");
            builder.Property(x => x.Color).HasColumnName("Color");

            builder.HasOne(pd => pd.Product).WithOne(p => p.ProductDetail)
                .HasForeignKey<ProductDetail>(pd => pd.ProductId);
        }
    }
}
