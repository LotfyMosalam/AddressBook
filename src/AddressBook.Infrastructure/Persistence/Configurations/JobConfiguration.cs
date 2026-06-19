using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.Infrastructure.Persistence.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(j => j.Name).IsUnique();

        builder.Navigation(j => j.AddressEntries)
            .HasField("_addressEntries")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
