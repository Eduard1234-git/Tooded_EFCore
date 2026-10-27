using Microsoft.EntityFrameworkCore;
namespace Tooded_EFCore;

public partial class FormMain : Form
{
    private readonly Panel sivu = new(), sisu = new();
    private readonly Label lblLogo = new(), lblKasutaja = new(), lblPealkiri = new();
    private readonly Button btnPood = new(), btnArve = new(), btnTooted = new(), btnTellimused = new(), btnValju = new();
    private readonly Label[] kpiVaartus = { new(), new(), new(), new() };
    private readonly Label[] kpiSilt = { new(), new(), new(), new() };
    private readonly Panel[] kpiPaneel = { new(), new(), new(), new() };
    private readonly BarChart chart = new();

    public FormMain()
    {
        InitializeComponent();
        Text = $"Peamenüü – {AktiivneKasutaja.Nimi}";
        ClientSize = new Size(1040, 640);
        StartPosition = FormStartPosition.CenterScreen;

        // ----- külgriba -----
        sivu.Dock = DockStyle.Left; sivu.Width = 240; sivu.BackColor = Theme.Panel;
        lblLogo.SetBounds(20, 24, 210, 40); lblLogo.Text = "🛒 Tooded Pood";
        lblLogo.Font = new Font("Segoe UI", 16, FontStyle.Bold); lblLogo.ForeColor = Theme.Accent;
        lblKasutaja.SetBounds(22, 66, 200, 24); lblKasutaja.ForeColor = Theme.Muted;
        lblKasutaja.Text = $"{AktiivneKasutaja.Nimi} · {AktiivneKasutaja.Roll}";

        SeaNupp(btnPood, "🏬  Pood", 120, true, (s, e) => Ava(new Form3()));
        SeaNupp(btnArve, "🧾  Ostukorv ja arve", 172, false, (s, e) => Ava(new ArveForm()));
        SeaNupp(btnTooted, "📦  Tootehaldus", 224, false, (s, e) => Ava(new Form2()));
        SeaNupp(btnTellimused, "📊  Tellimused", 276, false, (s, e) => Ava(new TellimusedForm()));
        SeaNupp(btnValju, "⏻  Välju", 560, false, (s, e) => Close());
        btnTooted.Enabled = AktiivneKasutaja.OnAdmin;
        btnTellimused.Enabled = AktiivneKasutaja.OnAdmin;
        sivu.Controls.AddRange(new Control[] { lblLogo, lblKasutaja, btnPood, btnArve, btnTooted, btnTellimused, btnValju });

        // ----- sisu -----
        sisu.Dock = DockStyle.Fill;
        lblPealkiri.SetBounds(28, 22, 500, 40); lblPealkiri.Text = "Ülevaade";
        lblPealkiri.Font = new Font("Segoe UI", 20, FontStyle.Bold);
        sisu.Controls.Add(lblPealkiri);

        string[] sildid = { "TOOTEID", "LAOVÄÄRTUS", "KÄIVE", "VÄHE LAOS (≤5)" };
        for (int i = 0; i < 4; i++)
        {
            kpiPaneel[i].SetBounds(28 + i * 188, 80, 176, 92);
            kpiPaneel[i].BackColor = Theme.Panel;
            kpiSilt[i].SetBounds(14, 12, 150, 20); kpiSilt[i].Text = sildid[i]; kpiSilt[i].ForeColor = Theme.Muted;
            kpiVaartus[i].SetBounds(14, 38, 156, 40);
            kpiVaartus[i].Font = new Font("Segoe UI", 18, FontStyle.Bold);
            kpiPaneel[i].Controls.AddRange(new Control[] { kpiSilt[i], kpiVaartus[i] });
            sisu.Controls.Add(kpiPaneel[i]);
        }
        chart.SetBounds(28, 192, 750, 420);
        chart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        chart.Pealkiri = "Laoväärtus kategooriate kaupa";
        sisu.Controls.Add(chart);

        Controls.Add(sisu);
        Controls.Add(sivu);
        Theme.Apply(this);
        btnPood.BackColor = Theme.Accent;
        LaeStatistika();
    }

    private void SeaNupp(Button nupp, string tekst, int y, bool primary, EventHandler kasitleja)
    {
        nupp.Text = tekst; nupp.TextAlign = ContentAlignment.MiddleLeft;
        nupp.Padding = new Padding(12, 0, 0, 0);
        nupp.SetBounds(16, y, 208, 44);
        if (primary) nupp.Tag = "primary";
        nupp.Click += kasitleja;
    }

    private void Ava(Form vorm)
    {
        using (vorm) vorm.ShowDialog(this);
        LaeStatistika();
    }

    private void LaeStatistika()
    {
        using var db = new ToodedContext();
        var tooted = db.Toodetabel.Include(t => t.Kategooria).AsNoTracking().ToList();
        decimal kaive = db.Tellimused.Sum(t => (decimal?)t.Kokku) ?? 0;
        kpiVaartus[0].Text = tooted.Count.ToString();
        kpiVaartus[1].Text = $"{tooted.Sum(t => t.Hind * t.Kogus):N0} €";
        kpiVaartus[2].Text = $"{kaive:N2} €";
        kpiVaartus[3].Text = tooted.Count(t => t.Kogus <= 5).ToString();
        kpiVaartus[3].ForeColor = Theme.Bad;
        chart.Andmed = tooted
            .GroupBy(t => t.Kategooria.Kategooria_nimetus)
            .Select(g => (g.Key, g.Sum(x => x.Hind * x.Kogus)))
            .OrderByDescending(x => x.Item2).ToArray();
        chart.Invalidate();
    }
}
