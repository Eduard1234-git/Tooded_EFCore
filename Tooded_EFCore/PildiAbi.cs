namespace Tooded_EFCore;

// Väike abiklass piltide salvestamiseks ja näitamiseks.
public static class PildiAbi
{
    private static string Kaust => Path.Combine(AppContext.BaseDirectory, "Images");

    public static string Salvesta(string algneFail)
    {
        Directory.CreateDirectory(Kaust);
        string uusNimi = Guid.NewGuid().ToString("N")[..8] + Path.GetExtension(algneFail);
        File.Copy(algneFail, Path.Combine(Kaust, uusNimi));
        return uusNimi;
    }

    public static string? Tee(string? nimi)
    {
        if (string.IsNullOrWhiteSpace(nimi)) return null;
        return Path.Combine(Kaust, nimi);
    }

    public static void Naita(PictureBox kast, string? fail)
    {
        Image? vana = kast.Image;
        kast.Image = null;
        vana?.Dispose();

        if (fail == null || !File.Exists(fail)) return;

        // Koopia ei hoia originaalfaili lukus.
        using var pilt = Image.FromFile(fail);
        kast.Image = new Bitmap(pilt);
    }
}
