using Microsoft.EntityFrameworkCore;
namespace Tooded_EFCore;

public partial class Form3 : Form
{
    private readonly ToodedContext _db = new();
    private List<Toode> _koik = new();
    private int? _kat;   // null = kõik kategooriad
    private readonly Panel ylariba = new();
    private readonly FlowLayoutPanel flowKat = new(), flowKaardid = new();
    private readonly TextBox txtOtsing = new();
    private readonly ComboBox comboJarjesta = new();
    private readonly Button btnKorv = new();

    public Form3()
    {
        InitializeComponent();
        Text = "Pood";
        ClientSize = new Size(1120, 720);
        StartPosition = FormStartPosition.CenterParent;

        flowKaardid.Dock = DockStyle.Fill; flowKaardid.AutoScroll = true; flowKaardid.Padding = new Padding(10);
        flowKat.Dock = DockStyle.Top; flowKat.Height = 54; flowKat.Padding = new Padding(10, 8, 10, 0);
        flowKat.WrapContents = true;
        ylariba.Dock = DockStyle.Top; ylariba.Height = 64; ylariba.BackColor = Theme.Panel;

        txtOtsing.SetBounds(16, 17, 320, 27); txtOtsing.PlaceholderText = "🔍 Otsi toodet...";
        txtOtsing.TextChanged += (s, e) => Render();
        comboJarjesta.SetBounds(350, 17, 190, 27);
        comboJarjesta.DropDownStyle = ComboBoxStyle.DropDownList;
        comboJarjesta.Items.AddRange(new object[] { "Nimi A–Z", "Hind ↑", "Hind ↓", "Suurimad soodustused" });
        comboJarjesta.SelectedIndex = 0;
        comboJarjesta.SelectedIndexChanged += (s, e) => Render();

        btnKorv.SetBounds(ClientSize.Width - 310, 11, 294, 42);
        btnKorv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnKorv.Tag = "primary";
        btnKorv.Click += (s, e) =>
        {
            using (var f = new ArveForm()) f.ShowDialog(this);
            LaeAndmed();   // ladu võis muutuda
        };
        ylariba.Controls.AddRange(new Control[] { txtOtsing, comboJarjesta, btnKorv });

        Controls.Add(flowKaardid);
        Controls.Add(flowKat);
        Controls.Add(ylariba);
        Theme.Apply(this);

        Ostukorv.Muutus += UuendaKorvi;
        UuendaKorvi();
        LaeAndmed();
    }

    private void UuendaKorvi() =>
        btnKorv.Text = $"🛒  Ostukorv ({Ostukorv.Tk})  ·  {Ostukorv.Kokku:F2} €";

    private void LaeAndmed()
    {
        _koik = _db.Toodetabel.AsNoTracking().Include(t => t.Kategooria).ToList();
        var kategooriad = _db.Kategooriatabel.AsNoTracking().OrderBy(k => k.Kategooria_nimetus).ToList();

        foreach (Control c in flowKat.Controls.Cast<Control>().ToList()) c.Dispose();
        flowKat.Controls.Clear();
        LisaChip(null, $"Kõik ({_koik.Count})");
        foreach (var k in kategooriad)
            LisaChip(k.Id, $"{k.Kategooria_nimetus} ({_koik.Count(t => t.KategooriaId == k.Id)})");
        UuendaChipid();
        Render();
    }

    private void LisaChip(int? kat, string tekst)
    {
        var chip = new Button { Text = tekst, Tag = kat, AutoSize = true, Height = 34,
                                Padding = new Padding(8, 0, 8, 0), Margin = new Padding(0, 0, 8, 6) };
        Theme.StyleButton(chip, false);
        chip.Click += (s, e) => { _kat = kat; UuendaChipid(); Render(); };
        flowKat.Controls.Add(chip);
    }

    private void UuendaChipid()
    {
        foreach (var b in flowKat.Controls.OfType<Button>())
            Theme.StyleButton(b, (b.Tag as int?) == _kat);
    }

    private void Render()
    {
        IEnumerable<Toode> q = _koik;
        if (_kat is int k) q = q.Where(t => t.KategooriaId == k);
        string s = txtOtsing.Text.Trim();
        if (s != "") q = q.Where(t => t.Toodenimetus.Contains(s, StringComparison.OrdinalIgnoreCase));
        q = comboJarjesta.SelectedIndex switch
        {
            1 => q.OrderBy(t => t.Hind),
            2 => q.OrderByDescending(t => t.Hind),
            3 => q.OrderByDescending(t => t.SoodustusProtsent ?? 0).ThenBy(t => t.Toodenimetus),
            _ => q.OrderBy(t => t.Toodenimetus)
        };

        TuhjendaKaardid();
        flowKaardid.SuspendLayout();
        foreach (var t in q) flowKaardid.Controls.Add(LooTootekaart(t));
        flowKaardid.ResumeLayout();
    }

    private void TuhjendaKaardid()
    {
        foreach (Control c in flowKaardid.Controls.Cast<Control>().ToList())
        {
            foreach (var pb in c.Controls.OfType<PictureBox>()) pb.Image?.Dispose();
            c.Dispose();
        }
        flowKaardid.Controls.Clear();
    }

    private Panel LooTootekaart(Toode t)
    {
        var kaart = new Panel { Size = new Size(200, 312), BackColor = Theme.Panel, Margin = new Padding(8) };
        var pilt = new PictureBox
        {
            Bounds = new Rectangle(5, 5, 190, 150), SizeMode = PictureBoxSizeMode.Zoom,
            Image = Kaustad.LaePilt(t.Pilt), Cursor = Cursors.Hand
        };
        if (t.SoodustusProtsent is int p)
            pilt.Controls.Add(new Label
            {
                Text = $"-{p}%", AutoSize = true, BackColor = Theme.Bad, ForeColor = Color.White,
                Padding = new Padding(5, 2, 5, 2), Location = new Point(6, 6),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            });
        var nimi = new Label { Bounds = new Rectangle(8, 160, 184, 44), Text = t.Toodenimetus,
                               Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        var hind = new Label { Bounds = new Rectangle(8, 205, 110, 30), Text = $"{t.Hind:F2} €",
                               Font = new Font("Segoe UI", 15, FontStyle.Bold), ForeColor = Theme.Accent };
        var vana = new Label();
        if (t.VanaHind is decimal v && v > t.Hind)
        {
            vana.Bounds = new Rectangle(112, 211, 84, 22); vana.Text = $"{v:F2} €";
            vana.Font = new Font("Segoe UI", 10, FontStyle.Strikeout); vana.ForeColor = Theme.Muted;
        }
        var ladu = new Label
        {
            Bounds = new Rectangle(8, 238, 184, 20),
            Text = t.Kogus == 0 ? "Otsas" : t.Kogus <= 5 ? $"Viimased {t.Kogus} tk" : "Laos olemas",
            ForeColor = t.Kogus == 0 ? Theme.Bad : t.Kogus <= 5 ? Color.Orange : Theme.Good
        };
        var kogus = new NumericUpDown
        {
            Bounds = new Rectangle(8, 268, 64, 30), Minimum = 1, Maximum = Math.Max(1, t.Kogus),
            Value = 1, Enabled = t.Kogus > 0
        };
        var nupp = new Button { Bounds = new Rectangle(80, 265, 112, 36), Text = "Lisa korvi",
                                Enabled = t.Kogus > 0, Tag = "primary" };

        nupp.Click += (s, e) =>
        {
            int soov = (int)kogus.Value;
            int korvis = Ostukorv.Ridad.FirstOrDefault(r => r.Toode.Id == t.Id)?.Kogus ?? 0;
            if (korvis + soov > t.Kogus)
            {
                MessageBox.Show($"Laos on ainult {t.Kogus} tk (korvis juba {korvis}).");
                return;
            }
            Ostukorv.Lisa(t, soov);
            nupp.Text = "✓ Lisatud";
            var tm = new System.Windows.Forms.Timer { Interval = 900 };
            tm.Tick += (a, b) => { nupp.Text = "Lisa korvi"; tm.Stop(); tm.Dispose(); };
            tm.Start();
        };
        pilt.Click += (s, e) =>
            MessageBox.Show($"{t.Toodenimetus}\nKategooria: {t.Kategooria.Kategooria_nimetus}\nHind: {t.Hind:F2} €\nLaos: {t.Kogus} tk");

        kaart.Controls.AddRange(new Control[] { pilt, nimi, hind, vana, ladu, kogus, nupp });
        Theme.Apply(kaart);
        return kaart;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        Ostukorv.Muutus -= UuendaKorvi;
        TuhjendaKaardid();
        _db.Dispose();
        base.OnFormClosed(e);
    }
}
