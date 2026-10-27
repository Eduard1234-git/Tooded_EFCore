using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToodedUl_IKTpv
{
    public class ToodedContext : DbContext
    {
        public ToodedContext()
            : base("name=ToodedConnection")
        {
        }

        public DbSet<Toode> Toodetabel { get; set; }

        public DbSet<Kategooria> Kategooriatabel { get; set; }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Toode>()
                .ToTable("Toodetabel");

            modelBuilder.Entity<Kategooria>()
                .ToTable("Kategooriatabel");

            modelBuilder.Entity<Toode>()
                .Property(t => t.Hind)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Kategooria>().HasKey(k => k.Id);

            modelBuilder.Entity<Kategooria>().Property(k => k.Kategooria_nimetus)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class ToodedInitializer
        : CreateDatabaseIfNotExists<ToodedContext>
    {
        protected override void Seed(ToodedContext db)
        {
            db.Kategooriatabel.Add(new Kategooria
            {
                Kategooria_nimetus = "Joogid",
                Kirjeldus = "Joogid"
            });

            db.Kategooriatabel.Add(new Kategooria
            {
                Kategooria_nimetus = "Puu- ja köögiviljad",
                Kirjeldus = "Puu- ja köögiviljad"
            });

            db.Kategooriatabel.Add(new Kategooria
            {
                Kategooria_nimetus = "Maiustused",
                Kirjeldus = "Maiustused"
            });

            base.Seed(db);
        }
    }
}