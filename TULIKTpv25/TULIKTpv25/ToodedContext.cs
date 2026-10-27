using Microsoft.EntityFrameworkCore;
namespace Tooded_EFCore;

public class ToodedContext : DbContext
{
    public DbSet<Toode> Toodetabel => Set<Toode>();
    public DbSet<Kategooria> Kategooriatabel => Set<Kategooria>();
    public DbSet<Kasutaja> Kasutajad => Set<Kasutaja>();
    public DbSet<Tellimus> Tellimused => Set<Tellimus>();
    public DbSet<TellimuseRida> TellimuseRead => Set<TellimuseRida>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB;Database=Tooded_DB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategooria>().ToTable("Kategooriatabel");
        modelBuilder.Entity<Toode>().ToTable("Toodetabel");
        modelBuilder.Entity<Kategooria>().HasData(SeedData.KategooriadList());
        modelBuilder.Entity<Toode>().HasData(SeedData.Tooted());
    }
}
