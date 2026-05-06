using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistences.EntityConfiguration;

internal class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students", "dbo");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1, 1);

        builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(100);


        builder.HasMany(s => s.Teachers).WithMany(t => t.Students);
    }
}