namespace Tooded_EFCore;

using Microsoft.EntityFrameworkCore;


internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Kaustad.Loo();

        using (var db = new ToodedContext())
        {
            db.Database.Migrate();
            LooAlgkasutajad(db);
            AlgTooted.Loo(db);
        }

        Application.Run(new LoginForm());
    }

    private static void LooAlgkasutajad(ToodedContext db)
    {
        if (db.Kasutajad.Any()) return;

        db.Kasutajad.AddRange(
            new Kasutaja { Nimi = "Admin", Email = "admin@admin.ee",
                           ParoolHash = PasswordHelper.Hash("admin123"), Roll = "admin" },
            new Kasutaja { Nimi = "Kasutaja", Email = "kasutaja@pood.ee",
                           ParoolHash = PasswordHelper.Hash("kasutaja123"), Roll = "kasutaja" });
        db.SaveChanges();
    }
}
