namespace Tooded_EFCore;

// Õppeandmed: hinnad on näidishinnad, mitte kaupluse hinnakiri.
public static class AlgTooted
{
    public static void Loo(ToodedContext db)
    {
        Kaustad.Loo();
        var kategooriad = db.Kategooriatabel.ToList();

        // Puu- ja köögiviljad
        Lisa(db, kategooriad, "Banaan (1 kg)", "Puu- ja köögiviljad", 40, 1.50m, "banaan.jpg");
        Lisa(db, kategooriad, "Õun (1 kg)", "Puu- ja köögiviljad", 50, 1.80m, "oun.jpg");
        Lisa(db, kategooriad, "Apelsin (1 kg)", "Puu- ja köögiviljad", 30, 2.20m, "apelsin.jpg");
        Lisa(db, kategooriad, "Pirn (1 kg)", "Puu- ja köögiviljad", 25, 2.40m, "pirn.jpg");
        Lisa(db, kategooriad, "Tomat (1 kg)", "Puu- ja köögiviljad", 35, 2.90m, "tomat.jpg");
        Lisa(db, kategooriad, "Roheline paprika (1 kg)", "Puu- ja köögiviljad", 20, 3.20m, "paprika.jpg");
        Lisa(db, kategooriad, "Kartul (1 kg)", "Puu- ja köögiviljad", 60, 0.90m, "kartul.jpg");
        Lisa(db, kategooriad, "Sidrun (500 g)", "Puu- ja köögiviljad", 30, 1.40m, "sidrun.jpg");
        Lisa(db, kategooriad, "Viinamari (500 g)", "Puu- ja köögiviljad", 20, 2.60m, "viinamari.jpg");
        Lisa(db, kategooriad, "Kurk (1 kg)", "Puu- ja köögiviljad", 30, 2.10m, "kurk.jpg");
        Lisa(db, kategooriad, "Porgand (1 kg)", "Puu- ja köögiviljad", 45, 0.80m, "porgand.jpg");
        Lisa(db, kategooriad, "Sibul (1 kg)", "Puu- ja köögiviljad", 40, 0.95m, "sibul.jpg");
        Lisa(db, kategooriad, "Brokoli (1 tk)", "Puu- ja köögiviljad", 15, 1.60m, "brokoli.jpg");
        Lisa(db, kategooriad, "Mandariin (1 kg)", "Puu- ja köögiviljad", 35, 2.50m, "mandariin.jpg");

        // Marjad
        Lisa(db, kategooriad, "Maasikas (250 g)", "Marjad", 20, 2.50m, "maasikas.jpg");
        Lisa(db, kategooriad, "Mustikas (125 g)", "Marjad", 18, 2.20m, "mustikas.jpg");
        Lisa(db, kategooriad, "Vaarikas (125 g)", "Marjad", 15, 2.80m, "vaarikas.jpg");
        Lisa(db, kategooriad, "Jõhvikas (200 g)", "Marjad", 20, 2.40m, "johvikas.jpg");
        Lisa(db, kategooriad, "Pohl (250 g)", "Marjad", 12, 2.70m, "pohl.jpg");
        Lisa(db, kategooriad, "Kirss (250 g)", "Marjad", 16, 3.50m, "kirss.jpg");

        // Eksootilised puuviljad
        Lisa(db, kategooriad, "Mango (1 tk)", "Eksootilised puuviljad", 15, 1.90m, "mango.jpg");
        Lisa(db, kategooriad, "Ananass (1 tk)", "Eksootilised puuviljad", 12, 2.90m, "ananass.jpg");
        Lisa(db, kategooriad, "Kiivi (500 g)", "Eksootilised puuviljad", 25, 2.30m, "kiivi.jpg");
        Lisa(db, kategooriad, "Avokaado (1 tk)", "Eksootilised puuviljad", 20, 1.20m, "avokaado.jpg");
        Lisa(db, kategooriad, "Granaatõun (1 tk)", "Eksootilised puuviljad", 10, 1.99m, "granaatoun.jpg");
        Lisa(db, kategooriad, "Kookospähkel (1 tk)", "Eksootilised puuviljad", 8, 2.49m, "kookos.jpg");

        // Joogid
        Lisa(db, kategooriad, "Piim 2,5% (1 l)", "Joogid", 40, 1.09m, "piim.jpg");
        Lisa(db, kategooriad, "Keefir (1 l)", "Joogid", 25, 1.29m, "keefir.jpg");
        Lisa(db, kategooriad, "Apelsinimahl (1 l)", "Joogid", 30, 2.19m, "apelsinimahl.jpg");
        Lisa(db, kategooriad, "Õunamahl (1 l)", "Joogid", 30, 1.89m, "ounamahl.jpg");
        Lisa(db, kategooriad, "Mineraalvesi (0,5 l)", "Joogid", 60, 0.69m, "vesi.jpg");
        Lisa(db, kategooriad, "Limonaad (0,5 l)", "Joogid", 35, 1.19m, "limonaad.jpg");
        Lisa(db, kategooriad, "Jahvatatud kohv (500 g)", "Joogid", 20, 5.99m, "kohv.jpg");
        Lisa(db, kategooriad, "Must tee (20 pakki)", "Joogid", 25, 1.79m, "tee.jpg");

        // Maiustused
        Lisa(db, kategooriad, "Piimašokolaad (100 g)", "Maiustused", 40, 1.49m, "sokolaad.jpg");
        Lisa(db, kategooriad, "Šokolaaditükkidega küpsised (200 g)", "Maiustused", 30, 1.99m, "kupsised.jpg");
        Lisa(db, kategooriad, "Kaerahelbeküpsised (300 g)", "Maiustused", 25, 1.69m, "kaerakupsis.jpg");
        Lisa(db, kategooriad, "Želeekommid (250 g)", "Maiustused", 30, 1.79m, "kommid.jpg");
        Lisa(db, kategooriad, "Šokolaadisõõrik (1 tk)", "Maiustused", 20, 0.99m, "soorik.jpg");
        Lisa(db, kategooriad, "Marmorkook (400 g)", "Maiustused", 12, 3.49m, "kook.jpg");
        Lisa(db, kategooriad, "Maasikajäätis vahvlitopsis", "Maiustused", 25, 1.29m, "jaatis.jpg");
        Lisa(db, kategooriad, "Taani saiake (1 tk)", "Maiustused", 18, 1.15m, "taanisai.jpg");

        db.SaveChanges();
    }

    private static void Lisa(ToodedContext db, List<Kategooria> kategooriad,
        string nimi, string kategooriaNimi, int kogus, decimal hind, string pilt)
    {
        // Pilt kopeeritakse rakenduse seest pildikausta (internetti pole vaja).
        string failinimi = "pood_" + pilt;
        KopeeriPilt(pilt, failinimi);

        var olemas = db.Toodetabel.FirstOrDefault(t => t.Toodenimetus == nimi);
        if (olemas != null)
        {
            // Vanad väikesed demopildid vahetatakse uute vastu,
            // aga admini enda valitud pilti ei puututa.
            if (olemas.Pilt != null && olemas.Pilt.StartsWith("algtooted_demo_"))
                olemas.Pilt = failinimi;
            return;
        }

        var kategooria = kategooriad.FirstOrDefault(k =>
            string.Equals(k.Kategooria_nimetus, kategooriaNimi,
                StringComparison.OrdinalIgnoreCase));

        if (kategooria == null)
        {
            kategooria = new Kategooria { Kategooria_nimetus = kategooriaNimi };
            db.Kategooriatabel.Add(kategooria);
            kategooriad.Add(kategooria);
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

    private static void KopeeriPilt(string pilt, string failinimi)
    {
        string siht = Path.Combine(Kaustad.Pildid, failinimi);
        if (File.Exists(siht)) return;

        using var allikas = typeof(AlgTooted).Assembly.GetManifestResourceStream(
            "Tooded_EFCore.SeedImages." + pilt)
            ?? throw new FileNotFoundException("Tootepilt puudub: " + pilt);
        using var fail = File.Create(siht);
        allikas.CopyTo(fail);
    }
}
