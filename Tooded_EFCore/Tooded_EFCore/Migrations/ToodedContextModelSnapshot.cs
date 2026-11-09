namespace Tooded_EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(ToodedContext))]
public class ToodedContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.6")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);
        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Tooded_EFCore.Kategooria", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<string>("Kategooria_nimetus").IsRequired().HasColumnType("nvarchar(max)");
            b.Property<string>("Kirjeldus").HasColumnType("nvarchar(max)");
            b.HasKey("Id");
            b.ToTable("Kategooriatabel");
            b.HasData(
                new { Id = 1, Kategooria_nimetus = "Joogid" },
                new { Id = 2, Kategooria_nimetus = "Puu- ja köögiviljad" },
                new { Id = 3, Kategooria_nimetus = "Maiustused" });
        });

        modelBuilder.Entity("Tooded_EFCore.Toode", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<decimal>("Hind").HasPrecision(10, 2).HasColumnType("decimal(10,2)");
            b.Property<int>("KategooriaId").HasColumnType("int");
            b.Property<int>("Kogus").HasColumnType("int");
            b.Property<string>("Pilt").HasColumnType("nvarchar(max)");
            b.Property<string>("Toodenimetus").IsRequired().HasColumnType("nvarchar(max)");
            b.HasKey("Id");
            b.HasIndex("KategooriaId");
            b.ToTable("Toodetabel");
        });

        modelBuilder.Entity("Tooded_EFCore.Kasutaja", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<string>("Email").IsRequired().HasColumnType("nvarchar(450)");
            b.Property<string>("Nimi").IsRequired().HasColumnType("nvarchar(max)");
            b.Property<string>("ParoolHash").IsRequired().HasColumnType("nvarchar(max)");
            b.Property<string>("Roll").IsRequired().HasColumnType("nvarchar(max)");
            b.HasKey("Id");
            b.HasIndex("Email").IsUnique();
            b.ToTable("Kasutajad");
        });

        modelBuilder.Entity("Tooded_EFCore.Toode", b =>
        {
            b.HasOne("Tooded_EFCore.Kategooria", "Kategooria")
                .WithMany("Tooded").HasForeignKey("KategooriaId")
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
            b.Navigation("Kategooria");
        });

        modelBuilder.Entity("Tooded_EFCore.Kategooria", b =>
        {
            b.Navigation("Tooded");
        });
    }
}
