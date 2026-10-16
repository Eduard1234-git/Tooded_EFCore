using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Tooded_EFCore;

public partial class Form2 : Form
{
    private readonly ToodedContext db = new ToodedContext();
    private string? valitudPilt;
    private int valitudId = 0;

    public Form2()
    {
        InitializeComponent();
        LaeKategooriad();
        LaeTooted();
    }

    private void LaeKategooriad()
    {
        comboKategooria.DataSource = db.Kategooriatabel.OrderBy(k => k.Id).ToList();
        comboKategooria.DisplayMember = "Kategooria_nimetus";
        comboKategooria.ValueMember = "Id";
        comboKategooria.SelectedIndex = -1;
    }

    private void LaeTooted()
    {
        dataGridViewTooted.DataSource = db.Toodetabel.AsNoTracking()
            .OrderBy(t => t.Id)
            .Select(t => new
            {
                t.Id,
                t.Toodenimetus,
                t.Kogus,
                t.Hind,
                Kategooria = t.Kategooria.Kategooria_nimetus,
                t.Pilt
            }).ToList();

        dataGridViewTooted.Columns["Toodenimetus"]!.HeaderText = "Toode";
        dataGridViewTooted.Columns["Kogus"]!.HeaderText = "Kogus";
        dataGridViewTooted.Columns["Hind"]!.HeaderText = "Hind (€)";
        dataGridViewTooted.Columns["Hind"]!.DefaultCellStyle.Format = "0.00";
        dataGridViewTooted.Columns["Pilt"]!.Visible = false;
        dataGridViewTooted.ClearSelection();
    }

    private bool KontrolliAndmeid(out int kogus, out float hind)
    {
        kogus = 0;
        hind = 0;

        if (string.IsNullOrWhiteSpace(txtNimetus.Text) ||
            !int.TryParse(txtKogus.Text, out kogus) || kogus < 0 ||
            !float.TryParse(txtHind.Text.Replace(',', '.'), NumberStyles.Float,
                CultureInfo.InvariantCulture, out hind) || !float.IsFinite(hind) || hind < 0 ||
            comboKategooria.SelectedValue is not int)
        {
            MessageBox.Show("Sisesta toote nimi, korrektne kogus ja hind ning vali kategooria.");
            return false;
        }

        return true;
    }

    private void btnValiPilt_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog();
        ofd.Filter = "Pildid|*.jpg;*.jpeg;*.png;*.bmp";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            valitudPilt = ofd.FileName;
            PildiAbi.Naita(picturePilt, valitudPilt);
        }
    }

    private void btnLisa_Click(object? sender, EventArgs e)
    {
        if (!KontrolliAndmeid(out int kogus, out float hind)) return;

        try
        {
            var uus = new Toode
            {
                Toodenimetus = txtNimetus.Text.Trim(),
                Kogus = kogus,
                Hind = hind,
                KategooriaId = (int)comboKategooria.SelectedValue!,
                Pilt = valitudPilt == null ? "" : PildiAbi.Salvesta(valitudPilt)
            };

            db.Toodetabel.Add(uus);
            db.SaveChanges();
            LaeTooted();
            TyhjendaValjad();
            MessageBox.Show("Toode lisatud!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lisamine ebaõnnestus: " + ex.Message);
        }
    }

    private void dataGridViewTooted_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        int id = (int)dataGridViewTooted.Rows[e.RowIndex].Cells["Id"].Value!;
        var toode = db.Toodetabel.AsNoTracking().FirstOrDefault(t => t.Id == id);
        if (toode == null) return;

        valitudId = toode.Id;
        txtNimetus.Text = toode.Toodenimetus;
        txtKogus.Text = toode.Kogus.ToString();
        txtHind.Text = toode.Hind.ToString("0.00");
        comboKategooria.SelectedValue = toode.KategooriaId;
        valitudPilt = null;
        PildiAbi.Naita(picturePilt, PildiAbi.Tee(toode.Pilt));
    }

    private void btnUuenda_Click(object? sender, EventArgs e)
    {
        if (valitudId == 0)
        {
            MessageBox.Show("Vali kõigepealt tabelist toode.");
            return;
        }
        if (!KontrolliAndmeid(out int kogus, out float hind)) return;

        try
        {
            var toode = db.Toodetabel.Find(valitudId);
            if (toode == null) return;

            toode.Toodenimetus = txtNimetus.Text.Trim();
            toode.Kogus = kogus;
            toode.Hind = hind;
            toode.KategooriaId = (int)comboKategooria.SelectedValue!;
            if (valitudPilt != null)
                toode.Pilt = PildiAbi.Salvesta(valitudPilt);

            db.SaveChanges();
            LaeTooted();
            TyhjendaValjad();
            MessageBox.Show("Toode uuendatud!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Uuendamine ebaõnnestus: " + ex.Message);
        }
    }

    private void btnKustuta_Click(object? sender, EventArgs e)
    {
        if (valitudId == 0)
        {
            MessageBox.Show("Vali kõigepealt tabelist toode.");
            return;
        }

        if (MessageBox.Show("Kas kustutada valitud toode?", "Kustutamine",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        try
        {
            var toode = db.Toodetabel.Find(valitudId);
            if (toode == null) return;
            db.Toodetabel.Remove(toode);
            db.SaveChanges();
            LaeTooted();
            TyhjendaValjad();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Kustutamine ebaõnnestus: " + ex.Message);
        }
    }

    private void btnArve_Click(object? sender, EventArgs e)
    {
        if (valitudId == 0)
        {
            MessageBox.Show("Vali kõigepealt tabelist toode.");
            return;
        }

        var toode = db.Toodetabel.AsNoTracking().FirstOrDefault(t => t.Id == valitudId);
        if (toode == null) return;

        using var arve = new ArveForm(toode);
        arve.ShowDialog();
    }

    private void TyhjendaValjad()
    {
        valitudId = 0;
        valitudPilt = null;
        txtNimetus.Clear();
        txtKogus.Clear();
        txtHind.Clear();
        comboKategooria.SelectedIndex = -1;
        PildiAbi.Naita(picturePilt, null);
        dataGridViewTooted.ClearSelection();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        db.Dispose();
        base.OnFormClosed(e);
    }
}
