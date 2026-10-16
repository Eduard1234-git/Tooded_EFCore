using Microsoft.EntityFrameworkCore;

namespace Tooded_EFCore;

public class ToodedContext : DbContext
{
    public DbSet<Toode> Toodetabel => Set<Toode>();
    public DbSet<Kategooria> Kategooriatabel => Set<Kategooria>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB;Database=Tooded_DB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategooria>().ToTable("Kategooriatabel");
        modelBuilder.Entity<Toode>().ToTable("Toodetabel");

        modelBuilder.Entity<Toode>()
            .HasOne(t => t.Kategooria)
            .WithMany(k => k.Tooded)
            .HasForeignKey(t => t.KategooriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
