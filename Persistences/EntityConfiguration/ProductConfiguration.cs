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


            //CREATE PROCEDURE usp_InsertProduct
            //    @Name       NVARCHAR(100),
            //@Price      DECIMAL(18, 2),
            //@Barcode NCHAR(10),
            //@CategoryId INT,
            //    @NewId      INT OUTPUT
            //AS
            //    BEGIN
            //SET NOCOUNT ON;

            //INSERT INTO Products(Name, Price, Barcode, CategoryId)
            //VALUES(@Name, @Price, @Barcode, @CategoryId);

            //SET @NewId = SCOPE_IDENTITY();

            //END


            //builder.InsertUsingStoredProcedure("usp_InsertProduct", sp =>
            //{
            //    sp.HasParameter(p => p.Name, x => x.HasName("Name"));

            //    sp.HasParameter(p => p.Price, x => x.HasName("Price"));
            //    sp.HasParameter(p => p.Barcode, x => x.HasName("Barcode"));
            //    sp.HasParameter(p => p.CategoryId, x => x.HasName("CategoryId"));

            //    sp.HasParameter(p => p.Id, x => x.HasName("NewId").IsOutput());
            //});


            builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);


            builder.HasOne(p => p.ProductDetail).WithOne(pd => pd.Product)
                .HasForeignKey<ProductDetail>(pd => pd.ProductId);
        }
    }
}
