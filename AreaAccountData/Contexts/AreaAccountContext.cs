using AreaAccountData.Models;
using Microsoft.EntityFrameworkCore;

namespace AreaAccountData.Contexts;

public sealed class AreaAccountContext : DbContext
{
    public DbSet<AreaAccount> AreaAccount { get; set; }
    public DbSet<Person> Person { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AreaAccount>()
            .HasMany(account => account.Tenants)
            .WithMany(person => person.AreaAccounts)
            .UsingEntity(builder => builder.ToTable("AreaAccountsToTenants"));
    }
}