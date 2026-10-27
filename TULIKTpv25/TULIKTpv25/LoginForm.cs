namespace Tooded_EFCore;

public partial class LoginForm : Form
{
    private readonly TextBox txtEmail = new(), txtParool = new();
    private readonly Button btnLogin = new();
    private readonly Label lblPealkiri = new(), lblVihje = new(), lblVeateade = new();

    public LoginForm()
    {
        InitializeComponent();
        Text = "Sisselogimine";
        ClientSize = new Size(340, 380);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        lblPealkiri.SetBounds(30, 30, 280, 50);
        lblPealkiri.Text = "🛒 Tooded Pood";
        lblPealkiri.Font = new Font("Segoe UI", 20, FontStyle.Bold);
        lblPealkiri.ForeColor = Theme.Accent;

        txtEmail.SetBounds(30, 110, 280, 27); txtEmail.PlaceholderText = "E-post";
        txtParool.SetBounds(30, 150, 280, 27); txtParool.PlaceholderText = "Parool";
        txtParool.UseSystemPasswordChar = true;

        btnLogin.SetBounds(30, 195, 280, 40); btnLogin.Text = "Logi sisse";
        btnLogin.Tag = "primary"; btnLogin.Click += btnLogin_Click;

        lblVeateade.SetBounds(30, 245, 280, 24); lblVeateade.ForeColor = Theme.Bad;
        lblVihje.SetBounds(30, 290, 280, 60);
        lblVihje.ForeColor = Theme.Muted;
        lblVihje.Text = "Demo: admin@admin.ee / admin123\nkasutaja@pood.ee / kasutaja123";

        AcceptButton = btnLogin;
        Controls.AddRange(new Control[] { lblPealkiri, txtEmail, txtParool, btnLogin, lblVeateade, lblVihje });
        Theme.Apply(this);
    }

    private void btnLogin_Click(object? sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim().ToLower();
        using var db = new ToodedContext();
        var kasutaja = db.Kasutajad.FirstOrDefault(k => k.Email == email);
        if (kasutaja == null || !PasswordHelper.Verify(txtParool.Text, kasutaja.ParoolHash))
        {
            lblVeateade.Text = "Vale e-post või parool.";
            txtParool.Clear();
            return;
        }
        AktiivneKasutaja.Nimi = kasutaja.Nimi;
        AktiivneKasutaja.Roll = kasutaja.Roll;

        var menyy = new FormMain();
        menyy.FormClosed += (s, a) => Close();
        menyy.Show();
        Hide();
    }
}
