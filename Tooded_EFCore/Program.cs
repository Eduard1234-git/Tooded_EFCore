using Microsoft.EntityFrameworkCore;

namespace Tooded_EFCore;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // EF Core loob andmebaasi migratsiooni abil.
        try
        {
            using var db = new ToodedContext();
            db.Database.Migrate();

            // Alustuseks mõned kategooriad, et ComboBox ei oleks tühi.
            if (!db.Kategooriatabel.Any())
            {
                db.Kategooriatabel.AddRange(
                    new Kategooria { Kategooria_nimetus = "Elektroonika", Kirjeldus = "Elektroonilised seadmed" },
                    new Kategooria { Kategooria_nimetus = "Riided", Kirjeldus = "Riided ja jalanõud" },
                    new Kategooria { Kategooria_nimetus = "Toit", Kirjeldus = "Toidukaubad" },
                    new Kategooria { Kategooria_nimetus = "Muud", Kirjeldus = "Muud tooted" }
                );
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Andmebaasiga ei saanud ühendust.\n" +
                "Kontrolli, kas SQL Server LocalDB on paigaldatud.\n\n" + ex.Message,
                "Andmebaasi viga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Application.Run(new Form1());
    }
}
