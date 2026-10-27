using System.Drawing.Drawing2D;
namespace Tooded_EFCore;

// Ise joonistatud tulpdiagramm (GDI+), ilma välise teegita.
public class BarChart : Control
{
    public string Pealkiri { get; set; } = "";
    public (string Nimi, decimal Vaartus)[] Andmed { get; set; } = Array.Empty<(string, decimal)>();

    public BarChart()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Theme.Panel);
        using var titleFont = new Font("Segoe UI", 12, FontStyle.Bold);
        using var font = new Font("Segoe UI", 10);
        using var muted = new SolidBrush(Theme.Muted);
        using var text = new SolidBrush(Theme.Text);
        g.DrawString(Pealkiri, titleFont, text, 18, 12);
        if (Andmed.Length == 0) return;

        decimal max = Math.Max(1, Andmed.Max(a => a.Vaartus));
        const int vasak = 190, parem = 110, ylal = 56;
        int rida = Math.Max(24, (Height - ylal - 12) / Andmed.Length);
        int laius = Width - vasak - parem;
        for (int i = 0; i < Andmed.Length; i++)
        {
            int y = ylal + i * rida;
            int pikkus = (int)(laius * (double)(Andmed[i].Vaartus / max));
            g.DrawString(Andmed[i].Nimi, font, muted, new RectangleF(18, y + 2, vasak - 24, rida));
            var r = new Rectangle(vasak, y + 3, Math.Max(4, pikkus), rida - 10);
            using var br = new LinearGradientBrush(r, Theme.Accent, Color.FromArgb(255, 180, 60), 0f);
            g.FillRectangle(br, r);
            g.DrawString($"{Andmed[i].Vaartus:N0} €", font, text, vasak + pikkus + 8, y + 2);
        }
    }
}
