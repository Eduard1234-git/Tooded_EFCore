namespace Tooded_EFCore;

// Õppeandmed: hinnad on näidishinnad, mitte kaupluse hinnakiri.
public static class AlgTooted
{
    public static void Loo(ToodedContext db)
    {
        Kaustad.Loo();
        var kategooriad = db.Kategooriatabel.ToList();

        Lisa(db, kategooriad, "Banaan (1 kg)", "Puu- ja köögiviljad", 40, 1.50m, "demo_01.jpg");
        Lisa(db, kategooriad, "Õun (1 kg)", "Puu- ja köögiviljad", 50, 1.80m, "demo_02.jpg");
        Lisa(db, kategooriad, "Apelsin (1 kg)", "Puu- ja köögiviljad", 30, 2.20m, "demo_03.jpg");
        Lisa(db, kategooriad, "Pirn (1 kg)", "Puu- ja köögiviljad", 25, 2.40m, "demo_04.jpg");
        Lisa(db, kategooriad, "Tomat (1 kg)", "Puu- ja köögiviljad", 35, 2.90m, "demo_05.jpg");
        Lisa(db, kategooriad, "Roheline paprika (1 kg)", "Puu- ja köögiviljad", 20, 3.20m, "demo_06.jpg");
        Lisa(db, kategooriad, "Kartul (1 kg)", "Puu- ja köögiviljad", 60, 0.90m, "demo_07.jpg");
        Lisa(db, kategooriad, "Maasikas (250 g)", "Marjad", 20, 2.50m, "demo_08.jpg");
        Lisa(db, kategooriad, "Mustikas (125 g)", "Marjad", 18, 2.20m, "demo_09.jpg");
        Lisa(db, kategooriad, "Vaarikas (125 g)", "Marjad", 15, 2.80m, "demo_10.jpg");
        Lisa(db, kategooriad, "Mango (1 tk)", "Eksootilised puuviljad", 15, 1.90m, "demo_11.jpg");
        Lisa(db, kategooriad, "Ananass (1 tk)", "Eksootilised puuviljad", 12, 2.90m, "demo_12.jpg");
        Lisa(db, kategooriad, "Kiivi (500 g)", "Eksootilised puuviljad", 25, 2.30m, "demo_13.jpg");
        Lisa(db, kategooriad, "Sidrun (500 g)", "Puu- ja köögiviljad", 30, 1.40m, "demo_14.jpg");
        Lisa(db, kategooriad, "Viinamari (500 g)", "Puu- ja köögiviljad", 20, 2.60m, "demo_15.jpg");

        db.SaveChanges();
    }

    private static void Lisa(ToodedContext db, List<Kategooria> kategooriad,
        string nimi, string kategooriaNimi, int kogus, decimal hind, string pilt)
    {
        // Sama nimega olemasoleva toote andmeid ei muudeta.
        if (db.Toodetabel.Any(t => t.Toodenimetus == nimi)) return;

        var kategooria = kategooriad.FirstOrDefault(k =>
            string.Equals(k.Kategooria_nimetus, kategooriaNimi,
                StringComparison.OrdinalIgnoreCase));

        if (kategooria == null)
        {
            kategooria = new Kategooria { Kategooria_nimetus = kategooriaNimi };
            db.Kategooriatabel.Add(kategooria);
            kategooriad.Add(kategooria);
        }

        // Pildid on rakenduse sees, internetti pole käivitamisel vaja.
        string failinimi = "algtooted_" + pilt;
        string siht = Path.Combine(Kaustad.Pildid, failinimi);
        if (!File.Exists(siht))
        {
            using var allikas = typeof(AlgTooted).Assembly.GetManifestResourceStream(
                "Tooded_EFCore.SeedImages." + pilt)
                ?? throw new FileNotFoundException("Tootepilt puudub: " + pilt);
            using var fail = File.Create(siht);
            allikas.CopyTo(fail);
        }

        db.Toodetabel.Add(new Toode
        {
            Toodenimetus = nimi,
            Kogus = kogus,
            Hind = hind,
            Pilt = failinimi,
            Kategooria = kategooria
        });
    }
}
