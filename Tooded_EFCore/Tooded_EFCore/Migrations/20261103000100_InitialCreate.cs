namespace Tooded_EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

[DbContext(typeof(ToodedContext))]
[Migration("20261103000100_InitialCreate")]
public class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Kategooriatabel",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Kategooria_nimetus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Kirjeldus = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_Kategooriatabel", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Toodetabel",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Toodenimetus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Kogus = table.Column<int>(type: "int", nullable: false),
                Hind = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                Pilt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                KategooriaId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Toodetabel", x => x.Id);
                table.ForeignKey(
                    name: "FK_Toodetabel_Kategooriatabel_KategooriaId",
                    column: x => x.KategooriaId,
                    principalTable: "Kategooriatabel",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Kategooriatabel",
            columns: new[] { "Id", "Kategooria_nimetus" },
            values: new object[,]
            {
                { 1, "Joogid" },
                { 2, "Puu- ja köögiviljad" },
                { 3, "Maiustused" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Toodetabel_KategooriaId", table: "Toodetabel", column: "KategooriaId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Toodetabel");
        migrationBuilder.DropTable(name: "Kategooriatabel");
    }

    protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
