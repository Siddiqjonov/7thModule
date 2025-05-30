using ContactManager.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.UserRoleId);

        builder.Property(ur => ur.UserRoleName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ur => ur.Description)
            .HasMaxLength(200);
    }
}
