namespace Tooded_EFCore;

using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

public partial class Form2 : Form
{
    private readonly ToodedContext _db = new();
    private string? valitudPilt;

    private readonly DataGridView dataGridViewTooted = new();

    private readonly TextBox txtNimetus = new();
    private readonly TextBox txtKogus = new();
    private readonly TextBox txtHind = new();

    private readonly ComboBox comboKategooria = new();
    private readonly PictureBox picturePilt = new();

    private readonly Button btnValiPilt = new();
    private readonly Button btnLisa = new();
    private readonly Button btnUuenda = new();
    private readonly Button btnKustuta = new();
    private readonly Button btnTyhjenda = new();
    private readonly Button btnLisaKategooria = new();

    public Form2()
    {
        InitializeComponent();
        LooLiides();
        LaeKategooriad();
        LaeTooted();

        if (!AktiivneKasutaja.OnAdmin)
        {
            btnLisa.Enabled = false;
            btnUuenda.Enabled = false;
            btnKustuta.Enabled = false;
            btnValiPilt.Enabled = false;
            btnLisaKategooria.Enabled = false;
        }
    }

    // Kasutajaliides
    private void LooLiides()
    {
        Text = "Toodete haldus";
        ClientSize = new Size(1000, 680);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Stiil.Taust;

        // Tabel toodetega
        dataGridViewTooted.SetBounds(20, 80, 680, 580);
        dataGridViewTooted.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
        dataGridViewTooted.MultiSelect = false;
        dataGridViewTooted.ReadOnly = true;
        dataGridViewTooted.AllowUserToAddRows = false;
        dataGridViewTooted.CellClick += dataGridViewTooted_CellClick;

        // Tabeli välimus: valge, ilma ridade päisteta, punane valik
        dataGridViewTooted.BackgroundColor = Color.White;
        dataGridViewTooted.BorderStyle = BorderStyle.None;
        dataGridViewTooted.RowHeadersVisible = false;
        dataGridViewTooted.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewTooted.EnableHeadersVisualStyles = false;
        dataGridViewTooted.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 55);
        dataGridViewTooted.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dataGridViewTooted.ColumnHeadersDefaultCellStyle.Font = Stiil.Kiri(10, true);
        dataGridViewTooted.ColumnHeadersHeight = 36;
        dataGridViewTooted.DefaultCellStyle.Font = Stiil.Kiri(10);
        dataGridViewTooted.DefaultCellStyle.SelectionBackColor = Stiil.Punane;
        dataGridViewTooted.AlternatingRowsDefaultCellStyle.BackColor = Stiil.Taust;
        dataGridViewTooted.RowTemplate.Height = 30;

        // Parem paneel vormiga
        var vorm = new Panel { Bounds = new Rectangle(720, 80, 260, 580), BackColor = Color.White };
        Stiil.Aar(vorm);

        var pealkiri = new Label { Text = "Toote andmed", Font = Stiil.Kiri(12, true), Bounds = new Rectangle(15, 10, 230, 28) };

        txtNimetus.SetBounds(15, 45, 230, 27);
        txtNimetus.PlaceholderText = "Toote nimetus";

        txtKogus.SetBounds(15, 80, 110, 27);
        txtKogus.PlaceholderText = "Kogus";

        txtHind.SetBounds(135, 80, 110, 27);
        txtHind.PlaceholderText = "Hind, nt 2,50";

        comboKategooria.SetBounds(15, 115, 230, 27);
        comboKategooria.DropDownStyle = ComboBoxStyle.DropDownList;

        foreach (var c in new Control[] { txtNimetus, txtKogus, txtHind, comboKategooria })
            c.Font = Stiil.Kiri(10);

        picturePilt.SetBounds(55, 152, 150, 130);
        picturePilt.SizeMode = PictureBoxSizeMode.Zoom;
        picturePilt.BorderStyle = BorderStyle.FixedSingle;

        SeaNupp(btnValiPilt, "Vali pilt", 290, Stiil.Hall, btnValiPilt_Click);
        SeaNupp(btnLisa, "Lisa toode", 345, Stiil.Roheline, btnLisa_Click);
        SeaNupp(btnUuenda, "Salvesta muudatused", 390, Stiil.Sinine, btnUuenda_Click);
        SeaNupp(btnKustuta, "Kustuta toode", 435, Stiil.Punane, btnKustuta_Click);
        SeaNupp(btnTyhjenda, "Tühjenda väljad", 490, Stiil.Hall, (s, e) => TyhjendaVorm());
        SeaNupp(btnLisaKategooria, "+ Uus kategooria", 535, Color.FromArgb(50, 50, 55), btnLisaKategooria_Click);

        vorm.Controls.AddRange(new Control[]
        {
            pealkiri,
            txtNimetus,
            txtKogus,
            txtHind,
            comboKategooria,
            picturePilt,
            btnValiPilt,
            btnLisa,
            btnUuenda,
            btnKustuta,
            btnTyhjenda,
            btnLisaKategooria
        });

        Controls.Add(dataGridViewTooted);
        Controls.Add(vorm);
        Controls.Add(Stiil.Pais("Toodete haldus"));
    }

    private void SeaNupp(
        Button nupp,
        string tekst,
        int y,
        Color varv,
        EventHandler kasitleja)
    {
        nupp.Text = tekst;
        nupp.SetBounds(15, y, 230, 36);
        Stiil.Nupp(nupp, varv);
        nupp.Click += kasitleja;
    }

    // Uue kategooria lisamine
    private void btnLisaKategooria_Click(object? sender, EventArgs e)
    {
        if (!AktiivneKasutaja.OnAdmin) return;

        using var aken = new Form
        {
            Text = "Uus kategooria",
            ClientSize = new Size(340, 130),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var txtKategooria = new TextBox
        {
            Bounds = new Rectangle(20, 20, 300, 27),
            PlaceholderText = "Kategooria nimetus"
        };

        var btnSalvesta = new Button
        {
            Text = "Salvesta",
            Bounds = new Rectangle(20, 70, 140, 35),
            DialogResult = DialogResult.OK
        };

        var btnLoobu = new Button
        {
            Text = "Loobu",
            Bounds = new Rectangle(180, 70, 140, 35),
            DialogResult = DialogResult.Cancel
        };

        aken.Controls.AddRange(new Control[]
        {
            txtKategooria,
            btnSalvesta,
            btnLoobu
        });

        aken.AcceptButton = btnSalvesta;
        aken.CancelButton = btnLoobu;

        if (aken.ShowDialog(this) != DialogResult.OK) return;

        string nimetus = txtKategooria.Text.Trim();

        if (string.IsNullOrWhiteSpace(nimetus))
        {
            MessageBox.Show("Sisesta kategooria nimetus.");
            return;
        }

        if (_db.Kategooriatabel.Any(
            k => k.Kategooria_nimetus == nimetus))
        {
            MessageBox.Show("Selline kategooria on juba olemas.");
            return;
        }

        var kategooria = new Kategooria
        {
            Kategooria_nimetus = nimetus
        };

        _db.Kategooriatabel.Add(kategooria);
        _db.SaveChanges();

        LaeKategooriad();
        comboKategooria.SelectedValue = kategooria.Id;

        MessageBox.Show("Kategooria lisatud!");
    }

    // Andmete laadimine
    private void LaeKategooriad()
    {
        comboKategooria.DisplayMember = "Kategooria_nimetus";
        comboKategooria.ValueMember = "Id";

        comboKategooria.DataSource = _db.Kategooriatabel
            .OrderBy(k => k.Kategooria_nimetus)
            .ToList();
    }

    private void LaeTooted()
    {
        dataGridViewTooted.DataSource = _db.Toodetabel
            .Select(t => new
            {
                t.Id,
                t.Toodenimetus,
                t.Kogus,
                t.Hind,
                t.KategooriaId,
                Kategooria = t.Kategooria.Kategooria_nimetus,
                t.Pilt
            })
            .ToList();

        dataGridViewTooted.Columns["KategooriaId"]!.Visible = false;
        dataGridViewTooted.Columns["Pilt"]!.Visible = false;
        dataGridViewTooted.Columns["Id"]!.FillWeight = 30;
        dataGridViewTooted.Columns["Toodenimetus"]!.HeaderText = "Toode";
        dataGridViewTooted.Columns["Toodenimetus"]!.FillWeight = 180;
        dataGridViewTooted.Columns["Hind"]!.DefaultCellStyle.Format = "F2";
        dataGridViewTooted.Columns["Kogus"]!.FillWeight = 60;
        dataGridViewTooted.Columns["Hind"]!.FillWeight = 60;
        dataGridViewTooted.Columns["Kategooria"]!.FillWeight = 150;
    }

    // Pildi valimine
    private void btnValiPilt_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "Pildid|*.jpg;*.jpeg;*.png"
        };

        if (ofd.ShowDialog() != DialogResult.OK) return;

        using var ajutine = Image.FromFile(ofd.FileName);
        var uusPilt = new Bitmap(ajutine);

        picturePilt.Image?.Dispose();
        picturePilt.Image = uusPilt;
        valitudPilt = ofd.FileName;
    }

    // Vormi andmete kontroll
    private bool LoeVorm(Toode toode)
    {
        if (string.IsNullOrWhiteSpace(txtNimetus.Text))
        {
            MessageBox.Show("Sisesta toote nimetus.");
            return false;
        }

        if (!int.TryParse(txtKogus.Text, out int kogus) || kogus < 0)
        {
            MessageBox.Show("Kogus peab olema täisarv (0 või rohkem).");
            return false;
        }

        if (!decimal.TryParse(
                txtHind.Text.Replace(',', '.'),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal hind) || hind < 0)
        {
            MessageBox.Show("Hind peab olema arv, nt 2,50.");
            return false;
        }

        if (comboKategooria.SelectedValue is not int kategooriaId)
        {
            MessageBox.Show("Vali kategooria.");
            return false;
        }

        toode.Toodenimetus = txtNimetus.Text.Trim();
        toode.Kogus = kogus;
        toode.Hind = hind;
        toode.KategooriaId = kategooriaId;

        if (valitudPilt != null)
        {
            toode.Pilt = Path.GetFileName(valitudPilt);
            string siht = Path.Combine(Kaustad.Pildid, toode.Pilt);

            if (!string.Equals(
                    Path.GetFullPath(valitudPilt),
                    siht,
                    StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(valitudPilt, siht, true);
            }
        }

        return true;
    }

    private int? ValitudId()
    {
        if (dataGridViewTooted.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vali tabelist toode.");
            return null;
        }

        return (int)dataGridViewTooted
            .SelectedRows[0].Cells["Id"].Value;
    }

    private void TyhjendaVorm()
    {
        txtNimetus.Clear();
        txtKogus.Clear();
        txtHind.Clear();
        valitudPilt = null;

        picturePilt.Image?.Dispose();
        picturePilt.Image = null;
    }

    // Toote lisamine
    private void btnLisa_Click(object? sender, EventArgs e)
    {
        var uus = new Toode();

        if (!LoeVorm(uus)) return;

        _db.Toodetabel.Add(uus);
        _db.SaveChanges();

        TyhjendaVorm();
        LaeTooted();
    }

    // Tabelirea valimine
    private void dataGridViewTooted_CellClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var rida = dataGridViewTooted.Rows[e.RowIndex];

        txtNimetus.Text =
            rida.Cells["Toodenimetus"].Value?.ToString();
        txtKogus.Text =
            rida.Cells["Kogus"].Value?.ToString();
        txtHind.Text =
            rida.Cells["Hind"].Value?.ToString();

        comboKategooria.SelectedValue =
            rida.Cells["KategooriaId"].Value;

        valitudPilt = null;

        picturePilt.Image?.Dispose();
        picturePilt.Image = Kaustad.LaePilt(
            rida.Cells["Pilt"].Value?.ToString()
        );
    }

    // Toote muutmine
    private void btnUuenda_Click(object? sender, EventArgs e)
    {
        if (ValitudId() is not int id) return;

        var toode = _db.Toodetabel.Find(id);

        if (toode == null || !LoeVorm(toode)) return;

        _db.SaveChanges();
        LaeTooted();
    }

    // Toote kustutamine
    private void btnKustuta_Click(object? sender, EventArgs e)
    {
        if (ValitudId() is not int id) return;

        if (MessageBox.Show(
                "Kas kustutan valitud toote?",
                "Kinnitus",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
        {
            return;
        }

        var toode = _db.Toodetabel.Find(id);

        if (toode == null) return;

        _db.Toodetabel.Remove(toode);
        _db.SaveChanges();

        TyhjendaVorm();
        LaeTooted();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        picturePilt.Image?.Dispose();
        picturePilt.Image = null;

        _db.Dispose();
        base.OnFormClosed(e);
    }
}