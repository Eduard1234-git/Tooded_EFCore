namespace Tooded_EFCore;

partial class ArveForm
{
    private System.ComponentModel.IContainer? components = null;
    private ComboBox comboToode = null!;
    private NumericUpDown numKogus = null!;
    private ListBox listOstukorv = null!;
    private Label lblKokku = null!;
    private Button btnLisaKorvi = null!;
    private Button btnEemalda = null!;
    private Button btnGenPDF = null!;
    private Button btnSaadaEmail = null!;
    private TextBox txtSaaja = null!;
    private TextBox txtSaatja = null!;
    private TextBox txtParool = null!;

    private Label pealkiri = null!;
    private Label lblToode = null!;
    private Label lblKogus = null!;
    private Label lblSaatja = null!;
    private Label lblParool = null!;
    private Label lblSaaja = null!;
    private Label lblGmail = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pealkiri = new Label();
        lblToode = new Label();
        lblKogus = new Label();
        lblSaatja = new Label();
        lblParool = new Label();
        lblSaaja = new Label();
        lblGmail = new Label();
        comboToode = new ComboBox();
        numKogus = new NumericUpDown();
        listOstukorv = new ListBox();
        lblKokku = new Label();
        btnLisaKorvi = new Button();
        btnEemalda = new Button();
        btnGenPDF = new Button();
        btnSaadaEmail = new Button();
        txtSaatja = new TextBox();
        txtParool = new TextBox();
        txtSaaja = new TextBox();
        ((System.ComponentModel.ISupportInitialize)numKogus).BeginInit();
        SuspendLayout();

        pealkiri.Name = "pealkiri";
        pealkiri.Text = "Ostukorv ja arve";
        pealkiri.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        pealkiri.Location = new Point(20, 15);
        pealkiri.Size = new Size(460, 37);

        lblToode.Name = "lblToode";
        lblToode.Text = "Toode:";
        lblToode.Location = new Point(20, 67);
        lblToode.Size = new Size(250, 22);
        comboToode.Name = "comboToode";
        comboToode.Location = new Point(20, 91);
        comboToode.Size = new Size(330, 28);
        comboToode.DropDownStyle = ComboBoxStyle.DropDownList;

        lblKogus.Name = "lblKogus";
        lblKogus.Text = "Kogus:";
        lblKogus.Location = new Point(365, 67);
        lblKogus.Size = new Size(100, 22);
        numKogus.Name = "numKogus";
        numKogus.Location = new Point(365, 91);
        numKogus.Size = new Size(97, 27);
        numKogus.Minimum = 1;
        numKogus.Maximum = 999;
        numKogus.Value = 1;

        btnLisaKorvi.Name = "btnLisaKorvi";
        btnLisaKorvi.Text = "Lisa korvi";
        btnLisaKorvi.Location = new Point(490, 88);
        btnLisaKorvi.Size = new Size(130, 34);
        btnLisaKorvi.Click += btnLisaKorvi_Click;

        listOstukorv.Name = "listOstukorv";
        listOstukorv.Location = new Point(20, 140);
        listOstukorv.Size = new Size(600, 235);
        listOstukorv.HorizontalScrollbar = true;

        lblKokku.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblKokku.Name = "lblKokku";
        lblKokku.Location = new Point(20, 388);
        lblKokku.Size = new Size(330, 32);

        btnEemalda.Name = "btnEemalda";
        btnEemalda.Text = "Eemalda valitud";
        btnEemalda.Location = new Point(460, 386);
        btnEemalda.Size = new Size(160, 36);
        btnEemalda.Click += btnEemalda_Click;

        btnGenPDF.Name = "btnGenPDF";
        btnGenPDF.Text = "Genereeri PDF";
        btnGenPDF.Location = new Point(20, 440);
        btnGenPDF.Size = new Size(600, 38);
        btnGenPDF.Click += btnGenPDF_Click;

        lblGmail.Name = "lblGmail";
        lblGmail.Text = "E-mailiga saatmine (Gmaili rakenduse parool, mitte tavaline parool):";
        lblGmail.Location = new Point(20, 496);
        lblGmail.Size = new Size(610, 24);

        lblSaatja.Name = "lblSaatja";
        lblSaatja.Text = "Saatja Gmail:";
        lblSaatja.Location = new Point(20, 533);
        lblSaatja.Size = new Size(180, 21);
        txtSaatja.Name = "txtSaatja";
        txtSaatja.Location = new Point(20, 558);
        txtSaatja.Size = new Size(285, 27);

        lblParool.Name = "lblParool";
        lblParool.Text = "Rakenduse parool:";
        lblParool.Location = new Point(330, 533);
        lblParool.Size = new Size(230, 21);
        txtParool.Name = "txtParool";
        txtParool.Location = new Point(330, 558);
        txtParool.Size = new Size(290, 27);
        txtParool.UseSystemPasswordChar = true;

        lblSaaja.Name = "lblSaaja";
        lblSaaja.Text = "Saaja e-mail:";
        lblSaaja.Location = new Point(20, 600);
        lblSaaja.Size = new Size(240, 21);
        txtSaaja.Name = "txtSaaja";
        txtSaaja.Location = new Point(20, 625);
        txtSaaja.Size = new Size(360, 27);

        btnSaadaEmail.Name = "btnSaadaEmail";
        btnSaadaEmail.Text = "Saada e-mailiga";
        btnSaadaEmail.Location = new Point(396, 621);
        btnSaadaEmail.Size = new Size(224, 36);
        btnSaadaEmail.Click += btnSaadaEmail_Click;

        ClientSize = new Size(645, 675);
        Controls.AddRange(new Control[] { pealkiri, lblToode, comboToode,
            lblKogus, numKogus, btnLisaKorvi, listOstukorv, lblKokku, btnEemalda,
            btnGenPDF, lblGmail, lblSaatja, txtSaatja, lblParool, txtParool,
            lblSaaja, txtSaaja, btnSaadaEmail });
        StartPosition = FormStartPosition.CenterParent;
        Text = "ArveForm - Ostukorv ja arve";
        ((System.ComponentModel.ISupportInitialize)numKogus).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
