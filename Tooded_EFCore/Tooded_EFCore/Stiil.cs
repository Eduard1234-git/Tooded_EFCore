namespace Tooded_EFCore;

// Ühised värvid ja abimeetodid, et kõik aknad näeksid välja ühtemoodi
public static class Stiil
{
    public static readonly Color Punane = Color.FromArgb(218, 41, 28);
    public static readonly Color TumePunane = Color.FromArgb(176, 30, 20);
    public static readonly Color Roheline = Color.FromArgb(34, 139, 34);
    public static readonly Color Sinine = Color.FromArgb(37, 99, 235);
    public static readonly Color Hall = Color.FromArgb(120, 120, 120);
    public static readonly Color Taust = Color.FromArgb(244, 244, 246);
    public static readonly Color Joon = Color.FromArgb(225, 225, 230);

    public static Font Kiri(float suurus, bool paks = false) =>
        new Font("Segoe UI", suurus, paks ? FontStyle.Bold : FontStyle.Regular);

    // Lame värviline nupp nagu poe äppides
    public static void Nupp(Button nupp, Color taust)
    {
        nupp.FlatStyle = FlatStyle.Flat;
        nupp.FlatAppearance.BorderSize = 0;
        nupp.BackColor = taust;
        nupp.ForeColor = Color.White;
        nupp.Font = Kiri(10, true);
        nupp.Cursor = Cursors.Hand;
    }

    // Valge nupp halli äärega (nt kategooriate menüüs)
    public static void HeleNupp(Button nupp)
    {
        nupp.FlatStyle = FlatStyle.Flat;
        nupp.FlatAppearance.BorderColor = Joon;
        nupp.BackColor = Color.White;
        nupp.ForeColor = Color.Black;
        nupp.Font = Kiri(10);
        nupp.Cursor = Cursors.Hand;
    }

    // Punane pealkirjariba akna ülaosas
    public static Panel Pais(string pealkiri)
    {
        var pais = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Punane };
        var silt = new Label
        {
            Text = pealkiri,
            ForeColor = Color.White,
            Font = Kiri(16, true),
            AutoSize = true,
            Location = new Point(20, 14)
        };
        pais.Controls.Add(silt);
        return pais;
    }

    // Joonistab paneelile õhukese halli ääre (kaardi efekt)
    public static void Aar(Panel paneel)
    {
        paneel.Paint += (s, e) =>
        {
            using var pliiats = new Pen(Joon);
            e.Graphics.DrawRectangle(pliiats, 0, 0, paneel.Width - 1, paneel.Height - 1);
        };
    }
}
