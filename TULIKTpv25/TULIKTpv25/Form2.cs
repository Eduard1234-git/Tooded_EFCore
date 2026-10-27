using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Tooded_EFCore;

public partial class Form2 : Form
{
    private readonly ToodedContext _db = new();
    private string? valitudPilt;
    private readonly DataGridView dataGridViewTooted = new();
    private readonly TextBox txtOtsing = new(), txtNimetus = new(), txtKogus = new(), txtHind = new(), txtVanaHind = new();
    private readonly ComboBox comboKategooria = new();
    private readonly PictureBox picturePilt = new();
    private readonly Button btnValiPilt = new(), btnLisa = new(), btnUuenda = new(),
                            btnKustuta = new(), btnTyhjenda = new(), btnCsv = new();
    private readonly Label lblInfo = new();

    public Form2()
    {
        InitializeComponent();
        LooLiides();
        LaeKategooriad();
        LaeTooted();
        if (!AktiivneKasutaja.OnAdmin)
            btnLisa.Enabled = btnUuenda.Enabled = btnKustuta.Enabled = btnValiPilt.Enabled = false;
    }

    private void LooLiides()
    {
        Text = "Toodete haldus";
        ClientSize = new Size(1120, 640);
        StartPosition = FormStartPosition.CenterParent;

        txtOtsing.SetBounds(12, 12, 360, 27); txtOtsing.PlaceholderText = "🔍 Otsi toodet...";
        txtOtsing.TextChanged += (s, e) => LaeTooted();
        lblInfo.SetBounds(390, 15, 330, 24); lblInfo.ForeColor = Theme.Muted;

        dataGridViewTooted.SetBounds(12, 48, 740, 580);
        dataGridViewTooted.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewTooted.MultiSelect = false;
        dataGridViewTooted.ReadOnly = true;
        dataGridViewTooted.AllowUserToAddRows = false;
        dataGridViewTooted.CellClick += dataGridViewTooted_CellClick;
        dataGridViewTooted.CellFormatting += Grid_CellFormatting;

        txtNimetus.SetBounds(780, 12, 320, 27); txtNimetus.PlaceholderText = "Toote nimetus";
        txtKogus.SetBounds(780, 50, 320, 27); txtKogus.PlaceholderText = "Kogus laos";
        txtHind.SetBounds(780, 88, 320, 27); txtHind.PlaceholderText = "Hind €, nt 2,50";
        txtVanaHind.SetBounds(780, 126, 320, 27); txtVanaHind.PlaceholderText = "Vana hind (soodustuse jaoks, valikuline)";
        comboKategooria.SetBounds(780, 164, 320, 27);
        comboKategooria.DropDownStyle = ComboBoxStyle.DropDownList;

        picturePilt.SetBounds(780, 204, 150, 150);
        picturePilt.SizeMode = PictureBoxSizeMode.Zoom;
        picturePilt.BorderStyle = BorderStyle.FixedSingle;

        SeaNupp(btnValiPilt, "🖼  Vali pilt", 204, false, btnValiPilt_Click, 940, 150);
        SeaNupp(btnLisa, "➕  Lisa", 372, true, btnLisa_Click);
        SeaNupp(btnUuenda, "✏  Uuenda", 418, false, btnUuenda_Click);
        SeaNupp(btnKustuta, "🗑  Kustuta", 464, false, btnKustuta_Click);
        SeaNupp(btnTyhjenda, "Tühjenda väljad", 510, false, (s, e) => TyhjendaVorm());
        SeaNupp(btnCsv, "⬇  Ekspordi CSV", 556, false, btnCsv_Click);

        Controls.AddRange(new Control[] { txtOtsing, lblInfo, dataGridViewTooted, txtNimetus, txtKogus,
            txtHind, txtVanaHind, comboKategooria, picturePilt, btnValiPilt, btnLisa, btnUuenda,
            btnKustuta, btnTyhjenda, btnCsv });
        Theme.Apply(this);
    }

    private void SeaNupp(Button nupp, string tekst, int y, bool primary, EventHandler kasitleja,
                         int x = 780, int laius = 320)
    {
        nupp.Text = tekst;
        nupp.SetBounds(x, y, laius, 38);
        if (primary) nupp.Tag = "primary";
        nupp.Click += kasitleja;
    }

    private void LaeKategooriad()
    {
        comboKategooria.DataSource = _db.Kategooriatabel.OrderBy(k => k.Kategooria_nimetus).ToList();
        comboKategooria.DisplayMember = "Kategooria_nimetus";
        comboKategooria.ValueMember = "Id";
    }

    private void LaeTooted()
    {
        string q = txtOtsing.Text.Trim().ToLower();
        var andmed = _db.Toodetabel.AsNoTracking()
            .Where(t => q == "" || t.Toodenimetus.ToLower().Contains(q))
            .OrderBy(t => t.Toodenimetus)
            .Select(t => new
            {
                t.Id, t.Toodenimetus, t.Kogus, t.Hind, t.VanaHind, t.KategooriaId,
                Kategooria = t.Kategooria.Kategooria_nimetus, t.Pilt
            }).ToList();
        dataGridViewTooted.DataSource = andmed;
        var c = dataGridViewTooted.Columns;
        c["KategooriaId"]!.Visible = false;
        c["Pilt"]!.Visible = false;
        c["Id"]!.Width = 50; c["Id"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        c["Toodenimetus"]!.HeaderText = "Nimetus";
        c["Hind"]!.DefaultCellStyle.Format = "N2";
        c["VanaHind"]!.DefaultCellStyle.Format = "N2";
        c["VanaHind"]!.HeaderText = "Vana hind";
        lblInfo.Text = $"{andmed.Count} toodet · punane = otsas, oranž = vähe";
    }

    private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dataGridViewTooted.Columns[e.ColumnIndex].Name != "Kogus") return;
        if (e.Value is int k)
            e.CellStyle.ForeColor = k == 0 ? Theme.Bad : k <= 5 ? Color.Orange : Theme.Good;
    }

    private void btnValiPilt_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Pildid|*.jpg;*.jpeg;*.png" };
        if (ofd.ShowDialog() != DialogResult.OK) return;
        valitudPilt = ofd.FileName;
        using var ajutine = Image.FromFile(valitudPilt);
        picturePilt.Image = new Bitmap(ajutine);
    }

    private static bool TryRaha(string tekst, out decimal v) =>
        decimal.TryParse(tekst.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out v);

    private bool LoeVorm(Toode toode)
    {
        if (string.IsNullOrWhiteSpace(txtNimetus.Text))
        { MessageBox.Show("Sisesta toote nimetus."); return false; }
        if (!int.TryParse(txtKogus.Text, out int kogus) || kogus < 0)
        { MessageBox.Show("Kogus peab olema täisarv (0 või rohkem)."); return false; }
        if (!TryRaha(txtHind.Text, out decimal hind) || hind < 0)
        { MessageBox.Show("Hind peab olema arv, nt 2,50."); return false; }

        decimal? vana = null;
        if (!string.IsNullOrWhiteSpace(txtVanaHind.Text))
        {
            if (!TryRaha(txtVanaHind.Text, out decimal v) || v <= hind)
            { MessageBox.Show("Vana hind peab olema arv ja suurem kui uus hind."); return false; }
            vana = v;
        }
        if (comboKategooria.SelectedValue is not int kategooriaId)
        { MessageBox.Show("Vali kategooria."); return false; }

        toode.Toodenimetus = txtNimetus.Text.Trim();
        toode.Kogus = kogus;
        toode.Hind = hind;
        toode.VanaHind = vana;
        toode.KategooriaId = kategooriaId;

        if (valitudPilt != null)
        {
            toode.Pilt = Path.GetFileName(valitudPilt);
            string siht = Path.Combine(Kaustad.Pildid, toode.Pilt);
            if (!string.Equals(Path.GetFullPath(valitudPilt), siht, StringComparison.OrdinalIgnoreCase))
                File.Copy(valitudPilt, siht, true);
        }
        return true;
    }

    private int? ValitudId()
    {
        if (dataGridViewTooted.SelectedRows.Count == 0)
        { MessageBox.Show("Vali tabelist toode."); return null; }
        return (int)dataGridViewTooted.SelectedRows[0].Cells["Id"].Value;
    }

    private void TyhjendaVorm()
    {
        txtNimetus.Clear(); txtKogus.Clear(); txtHind.Clear(); txtVanaHind.Clear();
        valitudPilt = null;
        picturePilt.Image = null;
    }

    private void btnLisa_Click(object? sender, EventArgs e)
    {
        var uus = new Toode();
        if (!LoeVorm(uus)) return;
        _db.Toodetabel.Add(uus);
        _db.SaveChanges();
        TyhjendaVorm();
        LaeTooted();
    }

    private void dataGridViewTooted_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var rida = dataGridViewTooted.Rows[e.RowIndex];
        txtNimetus.Text = rida.Cells["Toodenimetus"].Value?.ToString();
        txtKogus.Text = rida.Cells["Kogus"].Value?.ToString();
        txtHind.Text = rida.Cells["Hind"].Value?.ToString();
        txtVanaHind.Text = rida.Cells["VanaHind"].Value?.ToString();
        comboKategooria.SelectedValue = rida.Cells["KategooriaId"].Value;
        valitudPilt = null;
        picturePilt.Image = Kaustad.LaePilt(rida.Cells["Pilt"].Value?.ToString());
    }

    private void btnUuenda_Click(object? sender, EventArgs e)
    {
        if (ValitudId() is not int id) return;
        var toode = _db.Toodetabel.Find(id);
        if (toode == null || !LoeVorm(toode)) return;
        _db.SaveChanges();
        LaeTooted();
    }

    private void btnKustuta_Click(object? sender, EventArgs e)
    {
        if (ValitudId() is not int id) return;
        if (MessageBox.Show("Kas kustutan valitud toote?", "Kinnitus", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        var toode = _db.Toodetabel.Find(id);
        if (toode == null) return;
        _db.Toodetabel.Remove(toode);
        _db.SaveChanges();
        TyhjendaVorm();
        LaeTooted();
    }

    private void btnCsv_Click(object? sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "tooted.csv" };
        if (sfd.ShowDialog() != DialogResult.OK) return;
        var sb = new StringBuilder("Id;Nimetus;Kogus;Hind;Kategooria\r\n");
        foreach (var t in _db.Toodetabel.AsNoTracking().Include(t => t.Kategooria).OrderBy(t => t.Id))
            sb.AppendLine($"{t.Id};{t.Toodenimetus};{t.Kogus};{t.Hind.ToString("F2", CultureInfo.InvariantCulture)};{t.Kategooria.Kategooria_nimetus}");
        File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
        MessageBox.Show("Eksporditud.");
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _db.Dispose();
        base.OnFormClosed(e);
    }
}
