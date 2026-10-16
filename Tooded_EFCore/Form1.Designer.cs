namespace Tooded_EFCore;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblPealkiri = null!;
    private Label lblJuhend = null!;
    private Button btnTooted = null!;
    private Button btnPood = null!;
    private Button btnArve = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblPealkiri = new Label();
        lblJuhend = new Label();
        btnTooted = new Button();
        btnPood = new Button();
        btnArve = new Button();
        SuspendLayout();

        lblPealkiri.Name = "lblPealkiri";
        lblPealkiri.Text = "Toodete haldus";
        lblPealkiri.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblPealkiri.Location = new Point(40, 25);
        lblPealkiri.Size = new Size(370, 45);

        lblJuhend.Name = "lblJuhend";
        lblJuhend.Text = "Vali, mida soovid teha:";
        lblJuhend.Location = new Point(43, 82);
        lblJuhend.Size = new Size(350, 30);

        btnTooted.Name = "btnTooted";
        btnTooted.Text = "Halda tooteid";
        btnTooted.Location = new Point(45, 125);
        btnTooted.Size = new Size(350, 50);
        btnTooted.Click += btnTooted_Click;

        btnPood.Name = "btnPood";
        btnPood.Text = "Ava pood";
        btnPood.Location = new Point(45, 187);
        btnPood.Size = new Size(350, 50);
        btnPood.Click += btnPood_Click;

        btnArve.Name = "btnArve";
        btnArve.Text = "Ostukorv ja arve";
        btnArve.Location = new Point(45, 249);
        btnArve.Size = new Size(350, 50);
        btnArve.Click += btnArve_Click;

        ClientSize = new Size(440, 340);
        Controls.AddRange(new Control[] { lblPealkiri, lblJuhend, btnTooted, btnPood, btnArve });
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tooted - avaleht";
        ResumeLayout(false);
    }
}
