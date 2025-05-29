using ContactSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactSystem.Infrastructure.Persistence.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(t => t.ContactId);
        builder.Property(t => t.Phone).IsRequired().HasMaxLength(15);
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Address).IsRequired(false);
        builder.Property(t => t.Email).IsRequired(false);
    }
}
