namespace Tooded_EFCore;

using System.Net;
using System.Net.Mail;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;


public partial class ArveForm : Form
{
    private readonly FlowLayoutPanel korviAla = new();
    private readonly Label lblTooteid = new(), lblKokku = new();
    private readonly TextBox txtEmail = new();
    private readonly Button btnGenPDF = new(), btnSaadaEmail = new(), btnTyhjenda = new();

    private string? viimanePdf;   // viimati loodud arve täistee

    public ArveForm()
    {
        InitializeComponent();
        Text = "Ostukorv";
        ClientSize = new Size(900, 600);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Stiil.Taust;

        // Vasakul korvi read
        korviAla.Bounds = new Rectangle(20, 80, 560, 500);
        korviAla.AutoScroll = true;
        korviAla.FlowDirection = FlowDirection.TopDown;
        korviAla.WrapContents = false;

        // Paremal kokkuvõte
        var kokkuvote = new Panel { Bounds = new Rectangle(600, 80, 280, 400), BackColor = Color.White };
        Stiil.Aar(kokkuvote);

        var pealkiri = new Label { Text = "Tellimuse kokkuvõte", Font = Stiil.Kiri(12, true), Bounds = new Rectangle(15, 15, 250, 25) };
        lblTooteid.Bounds = new Rectangle(15, 50, 250, 22);
        lblTooteid.Font = Stiil.Kiri(10);
        lblTooteid.ForeColor = Stiil.Hall;

        var kokkuSilt = new Label { Text = "Kokku", Font = Stiil.Kiri(11), Bounds = new Rectangle(15, 85, 100, 30) };
        lblKokku.Bounds = new Rectangle(110, 78, 155, 38);
        lblKokku.Font = Stiil.Kiri(18, true);
        lblKokku.TextAlign = ContentAlignment.MiddleRight;

        var emailSilt = new Label { Text = "Arve saaja e-post", Font = Stiil.Kiri(9), ForeColor = Stiil.Hall, Bounds = new Rectangle(15, 135, 250, 20) };
        txtEmail.Bounds = new Rectangle(15, 155, 250, 28);
        txtEmail.Font = Stiil.Kiri(10);
        txtEmail.PlaceholderText = "nimi@näide.ee";

        SeaNupp(btnGenPDF, "Koosta PDF arve", 200, Stiil.Punane, btnGenPDF_Click);
        SeaNupp(btnSaadaEmail, "Saada arve e-postiga", 250, Stiil.Sinine, btnSaadaEmail_Click);
        SeaNupp(btnTyhjenda, "Tühjenda korv", 330, Stiil.Hall, btnTyhjenda_Click);

        kokkuvote.Controls.AddRange(new Control[] { pealkiri, lblTooteid, kokkuSilt, lblKokku,
            emailSilt, txtEmail, btnGenPDF, btnSaadaEmail, btnTyhjenda });

        Controls.Add(korviAla);
        Controls.Add(kokkuvote);
        Controls.Add(Stiil.Pais("Ostukorv"));

        NaitaKorvi();
    }

    private void SeaNupp(Button nupp, string tekst, int y, Color varv, EventHandler kasitleja)
    {
        nupp.Text = tekst;
        nupp.Bounds = new Rectangle(15, y, 250, 40);
        Stiil.Nupp(nupp, varv);
        nupp.Click += kasitleja;
    }

    private void NaitaKorvi()
    {
        foreach (Control c in korviAla.Controls.Cast<Control>().ToList())
        {
            foreach (var pb in c.Controls.OfType<PictureBox>())
                pb.Image?.Dispose();
            c.Dispose();
        }
        korviAla.Controls.Clear();

        foreach (var rida in Ostukorv.Ridad)
            korviAla.Controls.Add(LooKorviRida(rida));

        if (Ostukorv.Ridad.Count == 0)
        {
            korviAla.Controls.Add(new Label
            {
                Text = "Ostukorv on tühi. Lisa poest mõni toode!",
                Font = Stiil.Kiri(12),
                ForeColor = Stiil.Hall,
                AutoSize = true,
                Margin = new Padding(10)
            });
        }

        UuendaKokku();
    }

    private void UuendaKokku()
    {
        lblTooteid.Text = $"Tooteid korvis: {Ostukorv.Ridad.Sum(r => r.Kogus)} tk";
        lblKokku.Text = $"{Ostukorv.Kokku:F2} €";
    }

    // Üks rida korvis: pilt, nimi, kogus, summa ja eemaldamise nupp
    private Panel LooKorviRida(OstukorviRida rida)
    {
        var paneel = new Panel { Size = new Size(530, 90), BackColor = Color.White, Margin = new Padding(0, 0, 0, 8) };
        Stiil.Aar(paneel);

        var pilt = new PictureBox
        {
            Bounds = new Rectangle(10, 10, 70, 70),
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = Kaustad.LaePilt(rida.Toode.Pilt)
        };

        var nimi = new Label { Text = rida.Toode.Toodenimetus, Font = Stiil.Kiri(10, true), Bounds = new Rectangle(90, 15, 240, 40) };
        var yhikuHind = new Label { Text = $"{rida.Toode.Hind:F2} € / tk", Font = Stiil.Kiri(9), ForeColor = Stiil.Hall, Bounds = new Rectangle(90, 55, 200, 20) };

        var kogus = new NumericUpDown
        {
            Bounds = new Rectangle(330, 30, 60, 30),
            Font = Stiil.Kiri(10),
            Minimum = 1,
            Maximum = Math.Max(rida.Kogus, rida.Toode.Kogus),
            Value = rida.Kogus
        };
        var summa = new Label
        {
            Text = $"{rida.Summa:F2} €",
            Font = Stiil.Kiri(11, true),
            TextAlign = ContentAlignment.MiddleRight,
            Bounds = new Rectangle(395, 30, 85, 30)
        };

        // Koguse muutmisel uuendame ainult summasid (rida ise jääb alles)
        kogus.ValueChanged += (s, e) =>
        {
            rida.Kogus = (int)kogus.Value;
            summa.Text = $"{rida.Summa:F2} €";
            UuendaKokku();
        };

        var eemalda = new Button { Text = "✕", Bounds = new Rectangle(488, 30, 30, 30) };
        Stiil.HeleNupp(eemalda);
        eemalda.ForeColor = Stiil.Punane;
        eemalda.Click += (s, e) =>
        {
            Ostukorv.Ridad.Remove(rida);
            BeginInvoke(NaitaKorvi);   // joonistame korvi uuesti pärast klikki
        };

        paneel.Controls.AddRange(new Control[] { pilt, nimi, yhikuHind, kogus, summa, eemalda });
        return paneel;
    }

    private void btnTyhjenda_Click(object? sender, EventArgs e)
    {
        if (Ostukorv.Ridad.Count == 0) return;
        if (MessageBox.Show("Kas tühjendan ostukorvi?", "Kinnitus", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            Ostukorv.Tyhjenda();
            NaitaKorvi();
        }
    }

    // ---------- PDF ----------
    private void btnGenPDF_Click(object? sender, EventArgs e)
    {
        if (Ostukorv.Ridad.Count == 0)
        {
            MessageBox.Show("Ostukorv on tühi.");
            return;
        }

        viimanePdf = Path.Combine(Kaustad.Arved, $"Arve_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

        using var dokument = new PdfDocument();
        PdfPage leht = dokument.AddPage();

        using (XGraphics gfx = XGraphics.FromPdfPage(leht))
        {
            var pealkiri = new XFont("Arial", 20, XFontStyle.Bold);
            var tavaline = new XFont("Arial", 12);
            var paks = new XFont("Arial", 14, XFontStyle.Bold);
            var punane = new XSolidBrush(XColor.FromArgb(218, 41, 28));

            // Punane riba üleval nagu äpis
            gfx.DrawRectangle(punane, 0, 0, leht.Width, 70);
            gfx.DrawString("TOODED pood – ARVE", pealkiri, XBrushes.White,
                new XRect(0, 20, leht.Width, 30), XStringFormats.TopCenter);
            gfx.DrawString($"Kuupäev: {DateTime.Now:dd.MM.yyyy HH:mm}", tavaline, XBrushes.Black, 40, 100);
            gfx.DrawString($"Klient: {AktiivneKasutaja.Nimi}", tavaline, XBrushes.Black, 40, 120);

            double y = 160;
            foreach (var rida in Ostukorv.Ridad)
            {
                gfx.DrawString(rida.ToString(), tavaline, XBrushes.Black, 40, y);
                y += 22;
            }

            gfx.DrawLine(XPens.Gray, 40, y, leht.Width - 40, y);
            gfx.DrawString($"Kokku: {Ostukorv.Kokku:F2} €", paks, XBrushes.Black, 40, y + 25);
        }

        dokument.Save(viimanePdf);
        MessageBox.Show($"PDF on loodud:\n{viimanePdf}");
    }

    // ---------- E-post ----------
    private void btnSaadaEmail_Click(object? sender, EventArgs e)
    {
        if (viimanePdf == null || !File.Exists(viimanePdf))
        {
            MessageBox.Show("Koosta enne PDF arve.");
            return;
        }

        string saaja = txtEmail.Text.Trim();
        if (!MailAddress.TryCreate(saaja, out _))
        {
            MessageBox.Show("Sisesta korrektne e-posti aadress.");
            return;
        }

        const string saatja = "eduardkotkass@gmail.com";
        const string rakenduseParool = "hehf drxt xcxn gnsk";   // vt 4.2, MITTE tavaline parool

        try
        {
            // using vabastab manuse, muidu jääb PDF-fail lukku
            using var kiri = new MailMessage(saatja, saaja, "Arve", "Arve on manuses.");
            kiri.Attachments.Add(new Attachment(viimanePdf));

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(saatja, rakenduseParool),
                EnableSsl = true
            };
            smtp.Send(kiri);

            MessageBox.Show("Arve saadetud! Aitäh ostu eest!");
            Ostukorv.Tyhjenda();
            NaitaKorvi();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga: " + ex.Message);
        }
    }
}
