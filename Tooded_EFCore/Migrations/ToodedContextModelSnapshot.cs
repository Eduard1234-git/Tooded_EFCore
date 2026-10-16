using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tooded_EFCore.Migrations;

[DbContext(typeof(ToodedContext))]
public partial class ToodedContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.6")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        modelBuilder.Entity("Tooded_EFCore.Kategooria", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .UseIdentityColumn();
            b.Property<string>("Kategooria_nimetus")
                .IsRequired()
                .HasColumnType("nvarchar(max)");
            b.Property<string>("Kirjeldus")
                .IsRequired()
                .HasColumnType("nvarchar(max)");
            b.HasKey("Id");
            b.ToTable("Kategooriatabel");
        });

        modelBuilder.Entity("Tooded_EFCore.Toode", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .UseIdentityColumn();
            b.Property<float>("Hind")
                .HasColumnType("real");
            b.Property<int>("KategooriaId")
                .HasColumnType("int");
            b.Property<int>("Kogus")
                .HasColumnType("int");
            b.Property<string>("Pilt")
                .IsRequired()
                .HasColumnType("nvarchar(max)");
            b.Property<string>("Toodenimetus")
                .IsRequired()
                .HasColumnType("nvarchar(max)");
            b.HasKey("Id");
            b.HasIndex("KategooriaId");
            b.ToTable("Toodetabel");
        });

        modelBuilder.Entity("Tooded_EFCore.Toode", b =>
        {
            b.HasOne("Tooded_EFCore.Kategooria", "Kategooria")
                .WithMany("Tooded")
                .HasForeignKey("KategooriaId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            b.Navigation("Kategooria");
        });

        modelBuilder.Entity("Tooded_EFCore.Kategooria", b =>
        {
            b.Navigation("Tooded");
        });
    }
}
