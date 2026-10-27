using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
namespace Tooded_EFCore;

// Joonistab tootepildid ise (gradient + emoji + nimi), et pood näeks hea välja ka ilma fotodeta.
public static class PildiGeneraator
{
    private static readonly (Color A, Color B)[] Varvid =
    {
        (Color.FromArgb(59,130,246), Color.FromArgb(14,165,233)),   // joogid
        (Color.FromArgb(34,197,94),  Color.FromArgb(132,204,22)),   // puu- ja köögiviljad
        (Color.FromArgb(236,72,153), Color.FromArgb(168,85,247)),   // maiustused
        (Color.FromArgb(14,165,233), Color.FromArgb(99,102,241)),   // piimatooted
        (Color.FromArgb(245,158,11), Color.FromArgb(217,119,6)),    // leib
        (Color.FromArgb(239,68,68),  Color.FromArgb(190,24,60)),    // liha ja kala
        (Color.FromArgb(120,113,108),Color.FromArgb(68,64,60)),     // kuivained
    };
    private static readonly string[] KatEmoji = { "🥤", "🥬", "🍫", "🥛", "🍞", "🥩", "🍝" };
    private static readonly (string Voti, string Emoji)[] Emojid =
    {
        ("Banaan","🍌"),("Õun","🍎"),("Kurk","🥒"),("Tomat","🍅"),("Kartul","🥔"),("Sidrun","🍋"),
        ("Porgand","🥕"),("Avokaado","🥑"),("Munad","🥚"),("Või ","🧈"),("juust","🧀"),("Croissant","🥐"),
        ("Lõhe","🐟"),("kohv","☕"),("Coca","🥤"),("mahl","🧃"),("vesi","💧"),("Monster","⚡"),
        ("šokolaad","🍫"),("Pringles","🥔"),("Lay's","🥔"),("riis","🍚"),("Spaghetti","🍝"),
        ("suhkur","🍬"),("Haribo","🍬"),("Jogurt","🍦"),("Kinder","🍫"),("Mars","🍫"),("Hakkliha","🥩"),
    };

    public static void TaidaPuuduvad(ToodedContext db)
    {
        foreach (var t in db.Toodetabel.AsNoTracking().ToList())
        {
            if (string.IsNullOrEmpty(t.Pilt)) continue;
            string tee = Path.Combine(Kaustad.Pildid, t.Pilt);
            if (!File.Exists(tee)) Loo(tee, t.Toodenimetus, t.KategooriaId);
        }
    }

    private static string Emoji(string nimi, int kat)
    {
        foreach (var (voti, e) in Emojid)
            if (nimi.Contains(voti, StringComparison.OrdinalIgnoreCase)) return e;
        return KatEmoji[(kat - 1 + KatEmoji.Length) % KatEmoji.Length];
    }

    public static void Loo(string tee, string nimi, int kat)
    {
        var (a, b) = Varvid[(kat - 1 + Varvid.Length) % Varvid.Length];
        using var bmp = new Bitmap(300, 300);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;
        using (var br = new LinearGradientBrush(new Rectangle(0, 0, 300, 300), a, b, 45f))
            g.FillRectangle(br, 0, 0, 300, 300);
        using (var ring = new SolidBrush(Color.FromArgb(45, 255, 255, 255)))
        {
            g.FillEllipse(ring, -50, -50, 220, 220);
            g.FillEllipse(ring, 160, 80, 220, 220);
        }
        using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        using (var ef = new Font("Segoe UI Emoji", 84))
            g.DrawString(Emoji(nimi, kat), ef, Brushes.White, new RectangleF(0, 10, 300, 200), sf);
        using (var bar = new SolidBrush(Color.FromArgb(130, 0, 0, 0)))
            g.FillRectangle(bar, 0, 215, 300, 85);
        using (var nf = new Font("Segoe UI", 15, FontStyle.Bold))
            g.DrawString(nimi, nf, Brushes.White, new RectangleF(10, 218, 280, 80), sf);
        bmp.Save(tee, ImageFormat.Png);
    }
}
