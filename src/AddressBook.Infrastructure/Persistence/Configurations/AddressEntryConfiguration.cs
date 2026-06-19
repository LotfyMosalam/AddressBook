using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.Infrastructure.Persistence.Configurations;

public class AddressEntryConfiguration : IEntityTypeConfiguration<AddressEntry>
{
    public void Configure(EntityTypeBuilder<AddressEntry> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.MobileNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        builder.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Password)
            .IsRequired();

        builder.Property(e => e.PhotoUrl)
            .HasMaxLength(500);

        // Age is computed from DateOfBirth — never persisted
        builder.Ignore(e => e.Age);

        builder.HasIndex(e => e.Email).IsUnique();

        builder.HasOne(e => e.Job)
            .WithMany(j => j.AddressEntries)
            .HasForeignKey(e => e.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.AddressEntries)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
