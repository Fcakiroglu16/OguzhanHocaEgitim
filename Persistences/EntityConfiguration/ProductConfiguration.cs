using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Persistences.EntityConfiguration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // select * from Products where id=5 => cluster index
            // select Name,Price, from Products where name='Laptop' => non-cluster index ( included column: name, price)


            builder.ToTable("Products", "dbo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(100);
            builder.Property(x => x.Price).HasColumnName("Price").HasPrecision(18, 2);
            builder.Property(x => x.Barcode).HasColumnName("Barcode").IsFixedLength().HasMaxLength(10);


            builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);


            builder.HasOne(p => p.ProductDetail).WithOne(pd => pd.Product)
                .HasForeignKey<ProductDetail>(pd => pd.ProductId);
        }
    }
}
