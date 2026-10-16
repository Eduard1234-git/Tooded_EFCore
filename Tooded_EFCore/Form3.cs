using Microsoft.EntityFrameworkCore;

namespace Tooded_EFCore;

public partial class Form3 : Form
{
    public Form3()
    {
        InitializeComponent();
        LaeKategooriadJaPildid();
    }

    private void LaeKategooriadJaPildid()
    {
        tabPood.TabPages.Clear();
        using var db = new ToodedContext();
        var kategooriad = db.Kategooriatabel.OrderBy(k => k.Id).ToList();

        foreach (var kat in kategooriad)
        {
            var leht = new TabPage(kat.Kategooria_nimetus);
            var paneel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            var tooted = db.Toodetabel.AsNoTracking()
                .Where(t => t.KategooriaId == kat.Id)
                .OrderBy(t => t.Toodenimetus)
                .ToList();

            foreach (var toode in tooted)
            {
                var kast = new Panel
                {
                    Size = new Size(173, 235),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(7)
                };
                var pilt = new PictureBox
                {
                    Location = new Point(8, 8),
                    Size = new Size(155, 156),
                    SizeMode = PictureBoxSizeMode.Zoom
                };
                PildiAbi.Naita(pilt, PildiAbi.Tee(toode.Pilt));

                var nimi = new Label
                {
                    Text = toode.Toodenimetus,
                    Location = new Point(7, 169),
                    Size = new Size(157, 30),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                var hind = new Label
                {
                    Text = $"{toode.Hind:0.00} €",
                    Location = new Point(7, 201),
                    Size = new Size(157, 25),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                pilt.Cursor = Cursors.Hand;
                pilt.Click += (s, e) => MessageBox.Show(
                    $"Toode: {toode.Toodenimetus}\nHind: {toode.Hind:0.00} €\nKogus: {toode.Kogus}",
                    "Toote info");

                kast.Controls.Add(pilt);
                kast.Controls.Add(nimi);
                kast.Controls.Add(hind);
                paneel.Controls.Add(kast);
            }

            leht.Controls.Add(paneel);
            tabPood.TabPages.Add(leht);
        }
    }
}
