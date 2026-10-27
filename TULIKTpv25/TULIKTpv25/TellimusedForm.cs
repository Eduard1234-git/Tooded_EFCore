using Microsoft.EntityFrameworkCore;
namespace Tooded_EFCore;

public partial class TellimusedForm : Form
{
    private readonly ToodedContext _db = new();
    private readonly DataGridView gridT = new(), gridR = new();
    private readonly Label lblT = new(), lblR = new();

    public TellimusedForm()
    {
        InitializeComponent();
        Text = "Tellimused (admin)";
        ClientSize = new Size(900, 620);
        StartPosition = FormStartPosition.CenterParent;

        lblT.SetBounds(12, 10, 600, 24); lblT.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        gridT.SetBounds(12, 40, 876, 250);
        lblR.SetBounds(12, 304, 600, 24); lblR.Text = "Tellimuse read"; lblR.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        gridR.SetBounds(12, 334, 876, 270);
        foreach (var g in new[] { gridT, gridR })
        { g.ReadOnly = true; g.AllowUserToAddRows = false; g.SelectionMode = DataGridViewSelectionMode.FullRowSelect; g.MultiSelect = false; }
        gridT.SelectionChanged += (s, e) => LaeRead();

        Controls.AddRange(new Control[] { lblT, gridT, lblR, gridR });
        Theme.Apply(this);
        LaeTellimused();

        if (!AktiivneKasutaja.OnAdmin) Shown += (s, e) => Close();
    }

    private void LaeTellimused()
    {
        var list = _db.Tellimused.AsNoTracking().OrderByDescending(t => t.Kuupaev)
            .Select(t => new { t.Id, t.Kuupaev, t.Klient, t.Kokku }).ToList();
        gridT.DataSource = list;
        gridT.Columns["Kuupaev"]!.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
        gridT.Columns["Kokku"]!.DefaultCellStyle.Format = "N2";
        lblT.Text = $"{list.Count} tellimust · käive kokku {list.Sum(x => x.Kokku):N2} €";
    }

    private void LaeRead()
    {
        if (gridT.CurrentRow?.Cells["Id"].Value is not int id) { gridR.DataSource = null; return; }
        gridR.DataSource = _db.TellimuseRead.AsNoTracking().Where(r => r.TellimusId == id)
            .Select(r => new { r.ToodeNimi, r.Kogus, r.Hind, Summa = r.Hind * r.Kogus }).ToList();
        gridR.Columns["Hind"]!.DefaultCellStyle.Format = "N2";
        gridR.Columns["Summa"]!.DefaultCellStyle.Format = "N2";
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _db.Dispose();
        base.OnFormClosed(e);
    }
}
