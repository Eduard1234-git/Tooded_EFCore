namespace Tooded_EFCore;

using Microsoft.EntityFrameworkCore;


// Poe peaaken: üleval otsing ja ostukorv, vasakul kategooriad, keskel tootekaardid
public partial class Form3 : Form
{
    private List<Toode> koikTooted = new();
    private List<Kategooria> kategooriad = new();
    private int? valitudKategooria;   // null = kõik tooted

    private readonly TextBox txtOtsing = new();
    private readonly Button btnOstukorv = new(), btnHaldus = new(), btnValja = new();
    private readonly FlowLayoutPanel kategooriaMenyy = new();
    private readonly FlowLayoutPanel tooteAla = new();
    private readonly Label lblPealkiri = new();

    public bool LogiValja { get; private set; }

    public Form3()
    {
        InitializeComponent();
        Text = "Tooded pood";
        ClientSize = new Size(1220, 760);
        MinimumSize = new Size(1100, 600);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Stiil.Taust;

        // Järjekord on oluline: viimasena lisatud Dock-paneel paigutatakse esimesena
        LooSisu();
        LooVasakMenyy();
        LooPais();

        LaeAndmed();
        UuendaKorviNuppu();
    }

    // ---------- Ülemine riba ----------
    private void LooPais()
    {
        // Laius kohe õigeks, muidu Anchor=Right nupud nihkuvad aknast välja
        var pais = new Panel { Dock = DockStyle.Top, Width = ClientSize.Width, Height = 70, BackColor = Stiil.Punane };

        var logo = new Label
        {
            Text = "TOODED pood",
            Font = Stiil.Kiri(18, true),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(20, 18)
        };

        txtOtsing.Bounds = new Rectangle(240, 20, 380, 30);
        txtOtsing.Font = Stiil.Kiri(11);
        txtOtsing.PlaceholderText = "Otsi tooteid...";
        txtOtsing.TextChanged += (s, e) => NaitaTooteid();

        var tere = new Label
        {
            Text = "Tere, " + AktiivneKasutaja.Nimi + "!",
            Font = Stiil.Kiri(10),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleRight,
            Bounds = new Rectangle(ClientSize.Width - 490, 25, 150, 22),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        // Nupud paremal servas (Anchor hoiab neid akna suurust muutes paremal)
        btnValja.Text = "Logi välja";
        btnValja.Bounds = new Rectangle(ClientSize.Width - 120, 18, 100, 34);
        btnValja.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        Stiil.Nupp(btnValja, Stiil.TumePunane);
        btnValja.Click += (s, e) => { LogiValja = true; Close(); };

        btnOstukorv.Bounds = new Rectangle(ClientSize.Width - 330, 18, 200, 34);
        btnOstukorv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        Stiil.Nupp(btnOstukorv, Color.White);
        btnOstukorv.ForeColor = Stiil.Punane;
        btnOstukorv.Click += btnOstukorv_Click;

        btnHaldus.Text = "Tootehaldus";
        btnHaldus.Bounds = new Rectangle(ClientSize.Width - 330, 18, 200, 34);
        btnHaldus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        Stiil.Nupp(btnHaldus, Color.White);
        btnHaldus.ForeColor = Stiil.Punane;
        btnHaldus.Click += btnHaldus_Click;

        // Admin haldab tooteid, tavakasutaja ostab
        btnHaldus.Visible = AktiivneKasutaja.OnAdmin;
        btnOstukorv.Visible = !AktiivneKasutaja.OnAdmin;

        pais.Controls.AddRange(new Control[] { logo, txtOtsing, tere, btnOstukorv, btnHaldus, btnValja });
        Controls.Add(pais);
    }

    // ---------- Vasak menüü kategooriatega ----------
    private void LooVasakMenyy()
    {
        var vasak = new Panel { Dock = DockStyle.Left, Width = 265, BackColor = Color.White, Padding = new Padding(10) };

        var silt = new Label
        {
            Text = "Kategooriad",
            Font = Stiil.Kiri(13, true),
            Dock = DockStyle.Top,
            Height = 40
        };

        kategooriaMenyy.Dock = DockStyle.Fill;
        kategooriaMenyy.FlowDirection = FlowDirection.TopDown;
        kategooriaMenyy.WrapContents = false;
        kategooriaMenyy.AutoScroll = true;

        vasak.Controls.Add(kategooriaMenyy);
        vasak.Controls.Add(silt);
        Controls.Add(vasak);
    }

    // ---------- Keskmine osa tootekaartidega ----------
    private void LooSisu()
    {
        var sisu = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15, 10, 10, 10) };

        lblPealkiri.Dock = DockStyle.Top;
        lblPealkiri.Height = 45;
        lblPealkiri.Font = Stiil.Kiri(16, true);

        tooteAla.Dock = DockStyle.Fill;
        tooteAla.AutoScroll = true;

        sisu.Controls.Add(tooteAla);
        sisu.Controls.Add(lblPealkiri);
        Controls.Add(sisu);
    }

    private void LaeAndmed()
    {
        // Uus DbContext iga kord, et näha adminni tehtud muudatusi
        using var db = new ToodedContext();
        koikTooted = db.Toodetabel.Include(t => t.Kategooria).OrderBy(t => t.Toodenimetus).ToList();
        kategooriad = db.Kategooriatabel.OrderBy(k => k.Kategooria_nimetus).ToList();

        LooKategooriaNupud();
        NaitaTooteid();
    }

    private void LooKategooriaNupud()
    {
        kategooriaMenyy.Controls.Clear();
        LisaKategooriaNupp("Kõik tooted", null, koikTooted.Count);

        foreach (var kat in kategooriad)
        {
            int arv = koikTooted.Count(t => t.KategooriaId == kat.Id);
            LisaKategooriaNupp(kat.Kategooria_nimetus, kat.Id, arv);
        }
    }

    private void LisaKategooriaNupp(string nimi, int? id, int arv)
    {
        var nupp = new Button
        {
            Text = $"{nimi} ({arv})",
            Size = new Size(240, 40),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 0, 0, 6)
        };

        // Valitud kategooria on punane
        if (id == valitudKategooria)
            Stiil.Nupp(nupp, Stiil.Punane);
        else
            Stiil.HeleNupp(nupp);

        nupp.Click += (s, e) =>
        {
            valitudKategooria = id;
            txtOtsing.Clear();
            LooKategooriaNupud();
            NaitaTooteid();
        };
        kategooriaMenyy.Controls.Add(nupp);
    }

    private void NaitaTooteid()
    {
        string otsing = txtOtsing.Text.Trim().ToLower();
        var tooted = koikTooted.AsEnumerable();

        if (otsing != "")
        {
            // otsing käib kõigi toodete seast
            tooted = tooted.Where(t => t.Toodenimetus.ToLower().Contains(otsing));
            lblPealkiri.Text = $"Otsing: \"{txtOtsing.Text.Trim()}\"";
        }
        else if (valitudKategooria != null)
        {
            tooted = tooted.Where(t => t.KategooriaId == valitudKategooria);
            lblPealkiri.Text = kategooriad.First(k => k.Id == valitudKategooria).Kategooria_nimetus;
        }
        else
        {
            lblPealkiri.Text = "Kõik tooted";
        }

        var nimekiri = tooted.ToList();
        lblPealkiri.Text += $"  ·  {nimekiri.Count} toodet";

        tooteAla.SuspendLayout();   // et kaardid ei vilguks lisamise ajal
        foreach (Control c in tooteAla.Controls.Cast<Control>().ToList())
        {
            foreach (var pb in c.Controls.OfType<PictureBox>())
                pb.Image?.Dispose();   // vabastame vanad pildid mälust
            c.Dispose();
        }
        tooteAla.Controls.Clear();

        foreach (var toode in nimekiri)
            tooteAla.Controls.Add(LooTootekaart(toode));

        if (nimekiri.Count == 0)
        {
            tooteAla.Controls.Add(new Label
            {
                Text = "Tooteid ei leitud.",
                Font = Stiil.Kiri(12),
                ForeColor = Stiil.Hall,
                AutoSize = true
            });
        }
        tooteAla.ResumeLayout();
    }

    // ---------- Üks tootekaart ----------
    private Panel LooTootekaart(Toode t)
    {
        var kaart = new Panel { Size = new Size(210, 340), BackColor = Color.White, Margin = new Padding(8) };
        Stiil.Aar(kaart);

        var pilt = new PictureBox
        {
            Bounds = new Rectangle(15, 12, 180, 150),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = Kaustad.LaePilt(t.Pilt),
            Cursor = Cursors.Hand
        };
        pilt.Click += (s, e) =>
            MessageBox.Show($"{t.Toodenimetus}\nKategooria: {t.Kategooria.Kategooria_nimetus}\nHind: {t.Hind:F2} €\nLaos: {t.Kogus} tk",
                "Toote info");

        var nimi = new Label
        {
            Text = t.Toodenimetus,
            Font = Stiil.Kiri(10),
            Bounds = new Rectangle(12, 166, 186, 50)
        };

        var hind = new Label
        {
            Text = $"{t.Hind:F2} €",
            Font = Stiil.Kiri(17, true),
            Bounds = new Rectangle(10, 218, 190, 36)
        };

        var laos = new Label
        {
            Font = Stiil.Kiri(9),
            Bounds = new Rectangle(12, 255, 186, 22)
        };
        if (t.Kogus > 0)
        {
            laos.Text = $"Laos {t.Kogus} tk";
            laos.ForeColor = Stiil.Roheline;
        }
        else
        {
            laos.Text = "Läbi müüdud";
            laos.ForeColor = Stiil.Punane;
        }

        kaart.Controls.AddRange(new Control[] { pilt, nimi, hind, laos });

        // Adminnile ostunuppe ei näidata
        if (!AktiivneKasutaja.OnAdmin)
        {
            var kogus = new NumericUpDown
            {
                Bounds = new Rectangle(12, 290, 55, 30),
                Font = Stiil.Kiri(11),
                Minimum = 1,
                Maximum = Math.Max(1, t.Kogus),
                Value = 1
            };

            var nupp = new Button
            {
                Text = "Lisa korvi",
                Bounds = new Rectangle(75, 288, 122, 34),
                Enabled = t.Kogus > 0
            };
            Stiil.Nupp(nupp, t.Kogus > 0 ? Stiil.Punane : Color.Silver);
            nupp.Click += (s, e) => LisaKorvi(t, (int)kogus.Value, nupp);

            kaart.Controls.AddRange(new Control[] { kogus, nupp });
        }

        return kaart;
    }

    private void LisaKorvi(Toode t, int kogus, Button nupp)
    {
        // Kontroll, et korvis ei oleks rohkem kui laos on
        var rida = Ostukorv.Ridad.FirstOrDefault(r => r.Toode.Id == t.Id);
        int juba = rida == null ? 0 : rida.Kogus;
        if (juba + kogus > t.Kogus)
        {
            MessageBox.Show($"Laos on ainult {t.Kogus} tk (korvis juba {juba} tk).");
            return;
        }

        Ostukorv.Lisa(t, kogus);
        UuendaKorviNuppu();

        // Lühike märguanne nupul MessageBoxi asemel
        nupp.Text = "✓ Lisatud";
        nupp.BackColor = Stiil.Roheline;
        var taimer = new System.Windows.Forms.Timer { Interval = 1000 };
        taimer.Tick += (s, e) =>
        {
            taimer.Stop();
            taimer.Dispose();
            if (nupp.IsDisposed) return;
            nupp.Text = "Lisa korvi";
            nupp.BackColor = Stiil.Punane;
        };
        taimer.Start();
    }

    private void UuendaKorviNuppu()
    {
        int arv = Ostukorv.Ridad.Sum(r => r.Kogus);
        btnOstukorv.Text = $"Ostukorv ({arv})  {Ostukorv.Kokku:F2} €";
    }

    private void btnOstukorv_Click(object? sender, EventArgs e)
    {
        using (var aken = new ArveForm())
            aken.ShowDialog(this);
        UuendaKorviNuppu();
    }

    private void btnHaldus_Click(object? sender, EventArgs e)
    {
        using (var aken = new Form2())
            aken.ShowDialog(this);
        LaeAndmed();   // admin võis tooteid muuta
    }
}
