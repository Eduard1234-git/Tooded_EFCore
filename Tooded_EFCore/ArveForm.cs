using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using System.Net;
using System.Net.Mail;

namespace Tooded_EFCore;

public partial class ArveForm : Form
{
    // Ostukorv on tavaline list - lihtne lahendus koolitöö jaoks.
    private readonly List<OstukorviRida> ostukorv = new List<OstukorviRida>();
    private string? viimanePdf;

    public ArveForm()
    {
        InitializeComponent();
        LaeTooted();
        UuendaSummat();
    }

    public ArveForm(Toode esialgneToode) : this()
    {
        comboToode.SelectedValue = esialgneToode.Id;
        LisaKorvi(esialgneToode, 1);
    }

    private void LaeTooted()
    {
        using var db = new ToodedContext();
        comboToode.DataSource = db.Toodetabel.AsNoTracking()
            .OrderBy(t => t.Toodenimetus).ToList();
        comboToode.DisplayMember = "Toodenimetus";
        comboToode.ValueMember = "Id";
        comboToode.SelectedIndex = -1;
    }

    private void LisaKorvi(Toode toode, int kogus)
    {
        if (kogus > toode.Kogus)
        {
            MessageBox.Show("Laos ei ole nii palju tooteid.");
            return;
        }

        var rida = new OstukorviRida
        {
            Nimetus = toode.Toodenimetus,
            Kogus = kogus,
            Hind = toode.Hind
        };
        ostukorv.Add(rida);
        listOstukorv.Items.Add(rida);
        viimanePdf = null;
        UuendaSummat();
    }

    private void btnLisaKorvi_Click(object? sender, EventArgs e)
    {
        if (comboToode.SelectedItem is not Toode toode)
        {
            MessageBox.Show("Vali kõigepealt toode.");
            return;
        }

        LisaKorvi(toode, (int)numKogus.Value);
    }

    private void btnEemalda_Click(object? sender, EventArgs e)
    {
        int koht = listOstukorv.SelectedIndex;
        if (koht < 0)
        {
            MessageBox.Show("Vali ostukorvist eemaldatav toode.");
            return;
        }

        ostukorv.RemoveAt(koht);
        listOstukorv.Items.RemoveAt(koht);
        viimanePdf = null;
        UuendaSummat();
    }

    private decimal Kokku()
    {
        return ostukorv.Sum(r => (decimal)r.Hind * r.Kogus);
    }

    private void UuendaSummat()
    {
        lblKokku.Text = $"Kokku: {Kokku():0.00} €";
    }

    private void btnGenPDF_Click(object? sender, EventArgs e)
    {
        if (ostukorv.Count == 0)
        {
            MessageBox.Show("Ostukorv on tühi.");
            return;
        }

        try
        {
            Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "Arved"));
            string fail = Path.Combine(AppContext.BaseDirectory, "Arved",
                $"Arve_{DateTime.Now:yyyyMMdd_HHmmss_fff}.pdf");

            // PdfSharpCore kasutab Windowsi fonte.
            if (GlobalFontSettings.FontResolver == null)
                GlobalFontSettings.FontResolver = new PdfSharpCore.Utils.FontResolver();

            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var pealkiri = new XFont("Arial", 20, XFontStyle.Bold);
            var tavaline = new XFont("Arial", 11, XFontStyle.Regular);
            var paks = new XFont("Arial", 11, XFontStyle.Bold);

            int y = 35;
            gfx.DrawString("ARVE", pealkiri, XBrushes.Black,
                new XRect(35, y, page.Width.Point - 70, 30), XStringFormats.TopCenter);
            y += 47;
            gfx.DrawString("Kuupäev: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                tavaline, XBrushes.Black, new XPoint(45, y));
            y += 40;
            gfx.DrawString("Toode", paks, XBrushes.Black, new XPoint(45, y));
            gfx.DrawString("Kogus", paks, XBrushes.Black, new XPoint(340, y));
            gfx.DrawString("Summa", paks, XBrushes.Black, new XPoint(455, y));
            y += 12;
            gfx.DrawLine(XPens.Black, 45, y, page.Width.Point - 45, y);
            y += 24;

            foreach (var rida in ostukorv)
            {
                // Kui tooteid on palju, jätkame järgmisel lehel.
                if (y > page.Height.Point - 90)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 55;
                }

                string nimi = rida.Nimetus.Length > 36
                    ? rida.Nimetus[..33] + "..." : rida.Nimetus;
                gfx.DrawString(nimi, tavaline, XBrushes.Black, new XPoint(45, y));
                gfx.DrawString(rida.Kogus.ToString(), tavaline, XBrushes.Black, new XPoint(340, y));
                gfx.DrawString($"{(decimal)rida.Hind * rida.Kogus:0.00} EUR",
                    tavaline, XBrushes.Black, new XPoint(455, y));
                y += 28;
            }

            if (y > page.Height.Point - 65)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                y = 55;
            }
            gfx.DrawLine(XPens.Black, 45, y, page.Width.Point - 45, y);
            y += 30;
            gfx.DrawString($"Kokku: {Kokku():0.00} EUR", paks,
                XBrushes.Black, new XPoint(340, y));

            document.Save(fail);
            viimanePdf = fail;
            MessageBox.Show("PDF salvestatud:\n" + fail);
        }
        catch (Exception ex)
        {
            MessageBox.Show("PDF loomine ebaõnnestus: " + ex.Message);
        }
    }

    private void btnSaadaEmail_Click(object? sender, EventArgs e)
    {
        if (viimanePdf == null || !File.Exists(viimanePdf))
        {
            MessageBox.Show("Kõigepealt genereeri PDF.");
            return;
        }

        if (!MailAddress.TryCreate(txtSaaja.Text.Trim(), out _) ||
            !MailAddress.TryCreate(txtSaatja.Text.Trim(), out _) ||
            string.IsNullOrWhiteSpace(txtParool.Text))
        {
            MessageBox.Show("Sisesta saatja Gmail, saaja e-mail ja Gmaili rakenduse parool.");
            return;
        }

        try
        {
            using var mail = new MailMessage();
            mail.From = new MailAddress(txtSaatja.Text.Trim());
            mail.To.Add(txtSaaja.Text.Trim());
            mail.Subject = "Arve";
            mail.Body = "Tere! Arve on manuses.";
            mail.Attachments.Add(new Attachment(viimanePdf));

            using var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(txtSaatja.Text.Trim(),
                txtParool.Text.Replace(" ", ""));
            smtp.Send(mail);

            MessageBox.Show("Arve saadetud!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("E-kirja saatmine ebaõnnestus:\n" + ex.Message);
        }
    }

    private class OstukorviRida
    {
        public string Nimetus { get; set; } = "";
        public int Kogus { get; set; }
        public float Hind { get; set; }

        public override string ToString()
        {
            return $"{Nimetus} - {Kogus} tk x {Hind:0.00} €";
        }
    }
}
