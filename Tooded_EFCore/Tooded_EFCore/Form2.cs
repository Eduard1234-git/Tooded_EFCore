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
        ClientSize = new Size(860, 600);

        dataGridViewTooted.SetBounds(12, 12, 600, 530);
        dataGridViewTooted.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
        dataGridViewTooted.MultiSelect = false;
        dataGridViewTooted.ReadOnly = true;
        dataGridViewTooted.AllowUserToAddRows = false;
        dataGridViewTooted.CellClick += dataGridViewTooted_CellClick;

        txtNimetus.SetBounds(630, 12, 200, 27);
        txtNimetus.PlaceholderText = "Toote nimetus";

        txtKogus.SetBounds(630, 50, 200, 27);
        txtKogus.PlaceholderText = "Kogus";

        txtHind.SetBounds(630, 88, 200, 27);
        txtHind.PlaceholderText = "Hind, nt 2,50";

        comboKategooria.SetBounds(630, 126, 200, 27);
        comboKategooria.DropDownStyle = ComboBoxStyle.DropDownList;

        picturePilt.SetBounds(630, 166, 150, 150);
        picturePilt.SizeMode = PictureBoxSizeMode.Zoom;
        picturePilt.BorderStyle = BorderStyle.FixedSingle;

        SeaNupp(btnValiPilt, "Vali pilt", 326, btnValiPilt_Click);
        SeaNupp(btnLisa, "Lisa", 376, btnLisa_Click);
        SeaNupp(btnUuenda, "Uuenda", 416, btnUuenda_Click);
        SeaNupp(btnKustuta, "Kustuta", 456, btnKustuta_Click);

        SeaNupp(
            btnTyhjenda,
            "Tühjenda väljad",
            506,
            (s, e) => TyhjendaVorm()
        );

        SeaNupp(
            btnLisaKategooria,
            "Lisa kategooria",
            550,
            btnLisaKategooria_Click
        );

        Controls.AddRange(new Control[]
        {
            dataGridViewTooted,
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
    }

    private void SeaNupp(
        Button nupp,
        string tekst,
        int y,
        EventHandler kasitleja)
    {
        nupp.Text = tekst;
        nupp.SetBounds(630, y, 200, 32);
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