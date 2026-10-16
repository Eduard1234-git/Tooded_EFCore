namespace Tooded_EFCore;

partial class Form2
{
    private System.ComponentModel.IContainer? components = null;
    private DataGridView dataGridViewTooted = null!;
    private TextBox txtNimetus = null!;
    private TextBox txtKogus = null!;
    private TextBox txtHind = null!;
    private ComboBox comboKategooria = null!;
    private PictureBox picturePilt = null!;
    private Button btnLisa = null!;
    private Button btnUuenda = null!;
    private Button btnKustuta = null!;
    private Button btnValiPilt = null!;
    private Button btnArve = null!;

    private Label pealkiri = null!;
    private Label lblNimetus = null!;
    private Label lblKogus = null!;
    private Label lblHind = null!;
    private Label lblKategooria = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pealkiri = new Label();
        lblNimetus = new Label();
        lblKogus = new Label();
        lblHind = new Label();
        lblKategooria = new Label();
        dataGridViewTooted = new DataGridView();
        txtNimetus = new TextBox();
        txtKogus = new TextBox();
        txtHind = new TextBox();
        comboKategooria = new ComboBox();
        picturePilt = new PictureBox();
        btnValiPilt = new Button();
        btnLisa = new Button();
        btnUuenda = new Button();
        btnKustuta = new Button();
        btnArve = new Button();
        ((System.ComponentModel.ISupportInitialize)dataGridViewTooted).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picturePilt).BeginInit();
        SuspendLayout();

        pealkiri.Name = "pealkiri";
        pealkiri.Text = "Toodete nimekiri";
        pealkiri.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        pealkiri.Location = new Point(14, 12);
        pealkiri.Size = new Size(400, 33);

        dataGridViewTooted.Name = "dataGridViewTooted";
        dataGridViewTooted.Location = new Point(14, 55);
        dataGridViewTooted.Size = new Size(605, 494);
        dataGridViewTooted.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        dataGridViewTooted.ReadOnly = true;
        dataGridViewTooted.MultiSelect = false;
        dataGridViewTooted.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridViewTooted.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewTooted.RowHeadersVisible = false;
        dataGridViewTooted.AllowUserToAddRows = false;
        dataGridViewTooted.AllowUserToDeleteRows = false;
        dataGridViewTooted.CellClick += dataGridViewTooted_CellClick;

        lblNimetus.Name = "lblNimetus";
        lblNimetus.Text = "Toote nimetus:";
        lblNimetus.Location = new Point(645, 35);
        lblNimetus.Size = new Size(310, 21);
        txtNimetus.Name = "txtNimetus";
        txtNimetus.Location = new Point(645, 59);
        txtNimetus.Size = new Size(314, 27);

        lblKogus.Name = "lblKogus";
        lblKogus.Text = "Kogus:";
        lblKogus.Location = new Point(645, 97);
        lblKogus.Size = new Size(310, 21);
        txtKogus.Name = "txtKogus";
        txtKogus.Location = new Point(645, 120);
        txtKogus.Size = new Size(314, 27);

        lblHind.Name = "lblHind";
        lblHind.Text = "Hind (€):";
        lblHind.Location = new Point(645, 157);
        lblHind.Size = new Size(310, 21);
        txtHind.Name = "txtHind";
        txtHind.Location = new Point(645, 180);
        txtHind.Size = new Size(314, 27);

        lblKategooria.Name = "lblKategooria";
        lblKategooria.Text = "Kategooria:";
        lblKategooria.Location = new Point(645, 216);
        lblKategooria.Size = new Size(310, 21);
        comboKategooria.Name = "comboKategooria";
        comboKategooria.Location = new Point(645, 240);
        comboKategooria.Size = new Size(314, 28);
        comboKategooria.DropDownStyle = ComboBoxStyle.DropDownList;

        picturePilt.Name = "picturePilt";
        picturePilt.Location = new Point(645, 283);
        picturePilt.Size = new Size(185, 145);
        picturePilt.BorderStyle = BorderStyle.FixedSingle;
        picturePilt.SizeMode = PictureBoxSizeMode.Zoom;

        btnValiPilt.Name = "btnValiPilt";
        btnValiPilt.Text = "Vali pilt...";
        btnValiPilt.Location = new Point(842, 283);
        btnValiPilt.Size = new Size(117, 38);
        btnValiPilt.Click += btnValiPilt_Click;

        btnLisa.Name = "btnLisa";
        btnLisa.Text = "Lisa";
        btnLisa.Location = new Point(645, 449);
        btnLisa.Size = new Size(97, 41);
        btnLisa.Click += btnLisa_Click;

        btnUuenda.Name = "btnUuenda";
        btnUuenda.Text = "Uuenda";
        btnUuenda.Location = new Point(751, 449);
        btnUuenda.Size = new Size(97, 41);
        btnUuenda.Click += btnUuenda_Click;

        btnKustuta.Name = "btnKustuta";
        btnKustuta.Text = "Kustuta";
        btnKustuta.Location = new Point(857, 449);
        btnKustuta.Size = new Size(102, 41);
        btnKustuta.Click += btnKustuta_Click;

        btnArve.Name = "btnArve";
        btnArve.Text = "Lisa valitud toode arvele";
        btnArve.Location = new Point(645, 508);
        btnArve.Size = new Size(314, 41);
        btnArve.Click += btnArve_Click;

        ClientSize = new Size(985, 570);
        Controls.AddRange(new Control[] { pealkiri, dataGridViewTooted,
            lblNimetus, txtNimetus, lblKogus, txtKogus, lblHind, txtHind,
            lblKategooria, comboKategooria, picturePilt, btnValiPilt,
            btnLisa, btnUuenda, btnKustuta, btnArve });
        StartPosition = FormStartPosition.CenterParent;
        Text = "Form2 - Toodete haldus";
        ((System.ComponentModel.ISupportInitialize)dataGridViewTooted).EndInit();
        ((System.ComponentModel.ISupportInitialize)picturePilt).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
