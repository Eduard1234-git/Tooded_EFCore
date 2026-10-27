using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ToodedUl_IKTpv
{
    public partial class Form2 : Form
    {
        private readonly DataGridView grid = new DataGridView();
        private readonly TextBox txtNimetus = new TextBox();
        private readonly TextBox txtKogus = new TextBox();
        private readonly TextBox txtHind = new TextBox();
        private readonly ComboBox cmbKategooria = new ComboBox();
        private readonly PictureBox pic = new PictureBox();

        private int valitudId = 0;
        private string valitudPilt = null;

        public Form2()
        {
            InitializeComponent();

            Text = "Toodete haldus";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            EhitaLiides();
            LaeKategooriad();
            LaeTooted();
        }

        private void EhitaLiides()
        {
            Label l1 = new Label { Text = "Nimetus", Left = 20, Top = 20, Width = 100 };
            txtNimetus.SetBounds(130, 20, 200, 25);

            Label l2 = new Label { Text = "Kogus", Left = 20, Top = 55, Width = 100 };
            txtKogus.SetBounds(130, 55, 200, 25);

            Label l3 = new Label { Text = "Hind (€)", Left = 20, Top = 90, Width = 100 };
            txtHind.SetBounds(130, 90, 200, 25);

            Label l4 = new Label { Text = "Kategooria", Left = 20, Top = 125, Width = 100 };
            cmbKategooria.SetBounds(130, 125, 200, 25);
            cmbKategooria.DropDownStyle = ComboBoxStyle.DropDownList;

            pic.SetBounds(350, 20, 160, 130);
            pic.BorderStyle = BorderStyle.FixedSingle;
            pic.SizeMode = PictureBoxSizeMode.Zoom;

            Button btnPilt = new Button { Text = "Vali pilt", Left = 350, Top = 160, Width = 100 };
            btnPilt.Click += (s, e) => ValiPilt();

            Button btnLisa = new Button { Text = "Lisa", Left = 20, Top = 200, Width = 90 };
            btnLisa.Click += (s, e) => Lisa();

            Button btnUuenda = new Button { Text = "Uuenda", Left = 120, Top = 200, Width = 90 };
            btnUuenda.Click += (s, e) => Uuenda();

            Button btnKustuta = new Button { Text = "Kustuta", Left = 220, Top = 200, Width = 90 };
            btnKustuta.Click += (s, e) => Kustuta();

            Button btnTyhjenda = new Button { Text = "Tühjenda", Left = 320, Top = 200, Width = 90 };
            btnTyhjenda.Click += (s, e) => Tyhjenda();

            grid.SetBounds(20, 250, 840, 280);
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionChanged += (s, e) => ValitudRida();

            Controls.AddRange(new Control[]
            {
                l1, txtNimetus, l2, txtKogus, l3, txtHind,
                l4, cmbKategooria, pic, btnPilt,
                btnLisa, btnUuenda, btnKustuta, btnTyhjenda, grid
            });
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LaeKategooriad();
            LaeTooted();
        }


        private void LaeKategooriad()
        {
            using (var db = new ToodedContext())
            {
                var kategooriad = db.Kategooriatabel
                    .ToList()
                    .OrderBy(k => k.Kategooria_nimetus)
                    .ToList();

                cmbKategooria.DataSource = kategooriad;
                cmbKategooria.DisplayMember = "Kategooria_nimetus";
                cmbKategooria.ValueMember = "Id";
            }
        }


        private void LaeTooted()
        {
            using (var db = new ToodedContext())
            {
                grid.DataSource = db.Toodetabel
                    .Select(t => new
                    {
                        t.Id,
                        Nimetus = t.Toodenimetus,
                        t.Kogus,
                        t.Hind,
                        Kategooria = t.Kategooria.Kategooria_nimetus,
                        t.Pilt
                    })
                    .ToList();
            }

            if (grid.Columns["Id"] != null)
                grid.Columns["Id"].Visible = false;

            if (grid.Columns["Pilt"] != null)
                grid.Columns["Pilt"].Visible = false;
        }

        private void Lisa()
        {
            if (!Kontrolli()) return;

            using (var db = new ToodedContext())
            {
                db.Toodetabel.Add(new Toode
                {
                    Toodenimetus = txtNimetus.Text.Trim(),
                    Kogus = int.Parse(txtKogus.Text),
                    Hind = decimal.Parse(txtHind.Text),
                    KategooriaId = (int)cmbKategooria.SelectedValue,
                    Pilt = valitudPilt
                });

                db.SaveChanges();
            }

            LaeTooted();
            Tyhjenda();
            MessageBox.Show("Toode lisatud!");
        }

        private void Uuenda()
        {
            if (valitudId == 0 || !Kontrolli()) return;

            using (var db = new ToodedContext())
            {
                var t = db.Toodetabel.Find(valitudId);
                if (t == null) return;

                t.Toodenimetus = txtNimetus.Text.Trim();
                t.Kogus = int.Parse(txtKogus.Text);
                t.Hind = decimal.Parse(txtHind.Text);
                t.KategooriaId = (int)cmbKategooria.SelectedValue;

                if (valitudPilt != null)
                    t.Pilt = valitudPilt;

                db.SaveChanges();
            }

            LaeTooted();
            Tyhjenda();
            MessageBox.Show("Toode uuendatud!");
        }

        private void Kustuta()
        {
            if (valitudId == 0) return;

            if (MessageBox.Show("Kas kustutada valitud toode?",
                "Kinnitus", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (var db = new ToodedContext())
            {
                var t = db.Toodetabel.Find(valitudId);
                if (t != null)
                {
                    db.Toodetabel.Remove(t);
                    db.SaveChanges();
                }
            }

            LaeTooted();
            Tyhjenda();
        }

        private bool Kontrolli()
        {
            int kogus;
            decimal hind;

            if (string.IsNullOrWhiteSpace(txtNimetus.Text))
            {
                MessageBox.Show("Sisesta toote nimetus.");
                return false;
            }

            if (!int.TryParse(txtKogus.Text, out kogus) || kogus < 0)
            {
                MessageBox.Show("Kogus peab olema null või suurem täisarv.");
                return false;
            }

            if (!decimal.TryParse(txtHind.Text, out hind) || hind < 0)
            {
                MessageBox.Show("Sisesta korrektne hind.");
                return false;
            }

            if (cmbKategooria.SelectedValue == null)
            {
                MessageBox.Show("Lisa kõigepealt kategooria.");
                return false;
            }

            return true;
        }

        private void ValitudRida()
        {
            if (grid.CurrentRow == null) return;

            var row = grid.CurrentRow;

            if (row.Cells["Id"].Value == null) return;

            valitudId = Convert.ToInt32(row.Cells["Id"].Value);
            txtNimetus.Text = Convert.ToString(row.Cells["Nimetus"].Value);
            txtKogus.Text = Convert.ToString(row.Cells["Kogus"].Value);
            txtHind.Text = Convert.ToString(row.Cells["Hind"].Value);

            using (var db = new ToodedContext())
            {
                var t = db.Toodetabel.Find(valitudId);
                if (t != null)
                {
                    cmbKategooria.SelectedValue = t.KategooriaId;

                    if (pic.Image != null)
                    {
                        pic.Image.Dispose();
                        pic.Image = null;
                    }

                    pic.Image = Kaustad.LaePilt(t.Pilt);
                }
            }

            valitudPilt = null;
        }

        private void ValiPilt()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Pildid|*.jpg;*.jpeg;*.png;*.bmp";

                if (dlg.ShowDialog() != DialogResult.OK) return;

                Kaustad.Loo();

                string nimi = Guid.NewGuid().ToString() +
                    Path.GetExtension(dlg.FileName);

                string siht = Path.Combine(Kaustad.Pildid, nimi);

                File.Copy(dlg.FileName, siht);

                valitudPilt = nimi;

                if (pic.Image != null)
                    pic.Image.Dispose();

                using (var img = Image.FromFile(siht))
                    pic.Image = new Bitmap(img);
            }
        }

        private void Tyhjenda()
        {
            valitudId = 0;
            valitudPilt = null;

            txtNimetus.Clear();
            txtKogus.Clear();
            txtHind.Clear();

            if (pic.Image != null)
            {
                pic.Image.Dispose();
                pic.Image = null;
            }

            grid.ClearSelection();
        }
    }
}