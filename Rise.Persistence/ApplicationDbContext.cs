using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.DomainClasses;

namespace Rise.Persistence;

/// <inheritdoc />
public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<UserTransaction> Transaction => Set<UserTransaction>();

    public DbSet<TransactionItem> TransactionItems => Set<TransactionItem>();

    public DbSet<Booking> Booking => Set<Booking>();

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // All columns in the database have a maxlength of 4000.
        // in NVARACHAR 4000 is the maximum length that can be indexed by a database.
        // Some columns need more length, but these can be set on the configuration level for that Entity in particular.
        configurationBuilder.Properties<string>().HaveMaxLength(4_000);
        // All decimals columns should have 2 digits after the comma
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<UserTransaction>(entity =>
        {
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Type).IsRequired();
        });

        modelBuilder.Entity<TransactionItem>(entity =>
        {
            entity.Property(e => e.TransactionID).IsRequired();
        });
    }

}

