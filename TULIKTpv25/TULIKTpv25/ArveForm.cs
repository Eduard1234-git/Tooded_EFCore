using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
namespace Tooded_EFCore;

public partial class ArveForm : Form
{
    private const decimal KmMaar = 0.24m;   // hinnad sisaldavad käibemaksu
    private readonly ListBox listOstukorv = new();
    private readonly Label lblKokku = new(), lblKm = new();
    private readonly TextBox txtEmail = new();
    private readonly Button btnEemalda = new(), btnTelli = new(), btnAvaPdf = new(), btnSaadaEmail = new();
    private string? viimanePdf;

    public ArveForm()
    {
        InitializeComponent();
        Text = "Ostukorv ja arve";
        ClientSize = new Size(840, 500);
        StartPosition = FormStartPosition.CenterParent;

        listOstukorv.SetBounds(20, 20, 500, 380);
        lblKokku.SetBounds(20, 415, 500, 36); lblKokku.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        lblKm.SetBounds(20, 454, 500, 24); lblKm.ForeColor = Theme.Muted;

        txtEmail.SetBounds(545, 20, 275, 27); txtEmail.PlaceholderText = "Saaja e-post";
        SeaNupp(btnEemalda, "Eemalda valitud rida", 70, false, btnEemalda_Click);
        SeaNupp(btnTelli, "✔  Telli ja loo PDF-arve", 118, true, btnTelli_Click);
        SeaNupp(btnAvaPdf, "Ava viimane PDF", 166, false, btnAvaPdf_Click);
        SeaNupp(btnSaadaEmail, "Saada e-postiga", 214, false, btnSaadaEmail_Click);

        Controls.AddRange(new Control[] { listOstukorv, lblKokku, lblKm, txtEmail,
                                          btnEemalda, btnTelli, btnAvaPdf, btnSaadaEmail });
        Theme.Apply(this);
        NaitaKorvi();
    }

    private void SeaNupp(Button nupp, string tekst, int y, bool primary, EventHandler kasitleja)
    {
        nupp.Text = tekst; nupp.SetBounds(545, y, 275, 42);
        if (primary) nupp.Tag = "primary";
        nupp.Click += kasitleja;
    }

    private void NaitaKorvi()
    {
        listOstukorv.Items.Clear();
        foreach (var rida in Ostukorv.Ridad) listOstukorv.Items.Add(rida);
        lblKokku.Text = $"Kokku: {Ostukorv.Kokku:F2} €";
        lblKm.Text = $"sh käibemaks {KmMaar:P0}: {Ostukorv.Kokku * KmMaar / (1 + KmMaar):F2} €";
    }

    private void btnEemalda_Click(object? sender, EventArgs e)
    {
        if (listOstukorv.SelectedItem is OstukorviRida rida) { Ostukorv.Eemalda(rida); NaitaKorvi(); }
    }

    // ---------- Tellimus: salvestus + laojääk + PDF ----------
    private void btnTelli_Click(object? sender, EventArgs e)
    {
        if (Ostukorv.Ridad.Count == 0) { MessageBox.Show("Ostukorv on tühi."); return; }
        Tellimus tellimus;
        try
        {
            using var db = new ToodedContext();
            using var tx = db.Database.BeginTransaction();
            tellimus = new Tellimus { Kuupaev = DateTime.Now, Klient = AktiivneKasutaja.Nimi, Kokku = Ostukorv.Kokku };
            foreach (var rida in Ostukorv.Ridad)
            {
                var toode = db.Toodetabel.Find(rida.Toode.Id);
                if (toode == null || toode.Kogus < rida.Kogus)
                {
                    MessageBox.Show($"Laos pole enam piisavalt: {rida.Toode.Toodenimetus}");
                    return;   // tx kaotatakse, midagi ei salvestata
                }
                toode.Kogus -= rida.Kogus;
                tellimus.Read.Add(new TellimuseRida { ToodeNimi = toode.Toodenimetus, Kogus = rida.Kogus, Hind = toode.Hind });
            }
            db.Tellimused.Add(tellimus);
            db.SaveChanges();
            tx.Commit();
        }
        catch (Exception ex) { MessageBox.Show("Viga tellimuse salvestamisel: " + ex.Message); return; }

        try
        {
            viimanePdf = LooPdf(tellimus);
            Ostukorv.Tyhjenda();
            NaitaKorvi();
            Process.Start(new ProcessStartInfo(viimanePdf) { UseShellExecute = true });
        }
        catch (Exception ex) { MessageBox.Show("Tellimus on salvestatud, aga PDF ebaõnnestus: " + ex.Message); }
    }

    private void btnAvaPdf_Click(object? sender, EventArgs e)
    {
        if (viimanePdf == null || !File.Exists(viimanePdf)) { MessageBox.Show("PDF-i pole veel loodud."); return; }
        Process.Start(new ProcessStartInfo(viimanePdf) { UseShellExecute = true });
    }

    private static XFont F(double suurus, bool paks = false) =>
        new("Arial", suurus, paks ? XFontStyle.Bold : XFontStyle.Regular,
            new XPdfFontOptions(PdfFontEncoding.Unicode));

    private string LooPdf(Tellimus t)
    {
        string tee = Path.Combine(Kaustad.Arved, $"Arve_{t.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        using var dok = new PdfDocument();
        PdfPage leht = dok.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(leht);
        double w = leht.Width.Point;

        var aken = new XSolidBrush(XColor.FromArgb(255, 106, 61));
        var tume = new XSolidBrush(XColor.FromArgb(40, 44, 56));
        var triip = new XSolidBrush(XColor.FromArgb(244, 245, 248));
        var must = XBrushes.Black;

        void Tekst(string s, XFont f, XBrush b, double x, double y, double laius, XStringFormat fmt) =>
            gfx.DrawString(s, f, b, new XRect(x, y, laius, 20), fmt);

        gfx.DrawRectangle(aken, 0, 0, w, 100);
        Tekst("ARVE", F(30, true), XBrushes.White, 40, 32, 300, XStringFormats.TopLeft);
        Tekst($"nr {t.Id:D5}", F(16, true), XBrushes.White, 40, 38, w - 80, XStringFormats.TopRight);
        Tekst($"Kuupäev: {t.Kuupaev:dd.MM.yyyy HH:mm}", F(12), must, 40, 120, 400, XStringFormats.TopLeft);
        Tekst($"Klient: {t.Klient}", F(12), must, 40, 140, 400, XStringFormats.TopLeft);

        double y = 180;
        gfx.DrawRectangle(tume, 40, y, w - 80, 26);
        Tekst("Toode", F(11, true), XBrushes.White, 48, y + 5, 260, XStringFormats.TopLeft);
        Tekst("Kogus", F(11, true), XBrushes.White, 330, y + 5, 50, XStringFormats.TopRight);
        Tekst("Hind", F(11, true), XBrushes.White, 390, y + 5, 70, XStringFormats.TopRight);
        Tekst("Summa", F(11, true), XBrushes.White, 470, y + 5, 77, XStringFormats.TopRight);
        y += 26;

        int i = 0;
        foreach (var r in t.Read)
        {
            if (y > 740)
            {
                gfx.Dispose();
                leht = dok.AddPage();
                gfx = XGraphics.FromPdfPage(leht);
                y = 50;
            }
            if (i++ % 2 == 0) gfx.DrawRectangle(triip, 40, y, w - 80, 22);
            string nimi = r.ToodeNimi.Length > 40 ? r.ToodeNimi[..39] + "…" : r.ToodeNimi;
            Tekst(nimi, F(11), must, 48, y + 4, 280, XStringFormats.TopLeft);
            Tekst(r.Kogus.ToString(), F(11), must, 330, y + 4, 50, XStringFormats.TopRight);
            Tekst($"{r.Hind:F2} €", F(11), must, 390, y + 4, 70, XStringFormats.TopRight);
            Tekst($"{r.Hind * r.Kogus:F2} €", F(11), must, 470, y + 4, 77, XStringFormats.TopRight);
            y += 22;
        }

        decimal km = t.Kokku * KmMaar / (1 + KmMaar);
        y += 16;
        Tekst($"Summa km-ta: {t.Kokku - km:F2} €", F(11), must, 300, y, 247, XStringFormats.TopRight);
        Tekst($"Käibemaks {KmMaar:P0}: {km:F2} €", F(11), must, 300, y + 20, 247, XStringFormats.TopRight);
        Tekst($"KOKKU: {t.Kokku:F2} €", F(16, true), aken, 250, y + 46, 297, XStringFormats.TopRight);
        Tekst("Täname ostu eest!", F(11), XBrushes.Gray, 0, 800, w, XStringFormats.TopCenter);

        gfx.Dispose();
        dok.Save(tee);
        return tee;
    }

    // ---------- E-post ----------
    private void btnSaadaEmail_Click(object? sender, EventArgs e)
    {
        if (viimanePdf == null || !File.Exists(viimanePdf)) { MessageBox.Show("Tee enne tellimus (PDF puudub)."); return; }
        string saaja = txtEmail.Text.Trim();
        if (!MailAddress.TryCreate(saaja, out _)) { MessageBox.Show("Sisesta korrektne e-posti aadress."); return; }

        const string saatja = "sinu.konto@gmail.com";
        const string rakenduseParool = "xxxx xxxx xxxx xxxx";   // Gmaili rakenduse parool, MITTE tavaline parool
        try
        {
            using var kiri = new MailMessage(saatja, saaja, "Arve – Tooded Pood", "Aitäh ostu eest! Arve on manuses.");
            kiri.Attachments.Add(new Attachment(viimanePdf));
            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(saatja, rakenduseParool),
                EnableSsl = true
            };
            smtp.Send(kiri);
            MessageBox.Show("Arve saadetud!");
        }
        catch (Exception ex) { MessageBox.Show("Viga: " + ex.Message); }
    }
}
