﻿using ContactManager.Domain.Entities;
using ContactManager.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Infrastructure.Persistence;

public class MainContext : DbContext
{
    public MainContext(DbContextOptions<MainContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
