namespace Tooded_EFCore;

public partial class LoginForm : Form
{
    private readonly TextBox txtEmail = new(), txtParool = new();
    private readonly Button btnLogin = new();
    private readonly Label lblVeateade = new();

    public LoginForm()
    {
        InitializeComponent();
        Text = "Tooded pood – sisselogimine";
        ClientSize = new Size(400, 460);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Stiil.Taust;

        // Punane ülaosa logoga
        var pais = new Panel { Dock = DockStyle.Top, Height = 130, BackColor = Stiil.Punane };
        var logo = new Label
        {
            Text = "TOODED pood",
            Font = Stiil.Kiri(24, true),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(0, 25, 400, 50)
        };
        var tervitus = new Label
        {
            Text = "Värsked tooted otse koju",
            Font = Stiil.Kiri(11),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Bounds = new Rectangle(0, 75, 400, 30)
        };
        pais.Controls.AddRange(new Control[] { logo, tervitus });

        // Valge kaart, kus on väljad
        var kaart = new Panel { Bounds = new Rectangle(30, 150, 340, 280), BackColor = Color.White };
        Stiil.Aar(kaart);

        var pealkiri = new Label
        {
            Text = "Logi sisse",
            Font = Stiil.Kiri(14, true),
            Bounds = new Rectangle(20, 15, 300, 30)
        };
        var lblEmail = new Label { Text = "E-post", Font = Stiil.Kiri(9), ForeColor = Stiil.Hall, Bounds = new Rectangle(20, 55, 300, 20) };
        txtEmail.Bounds = new Rectangle(20, 75, 300, 30);
        txtEmail.Font = Stiil.Kiri(11);

        var lblParool = new Label { Text = "Parool", Font = Stiil.Kiri(9), ForeColor = Stiil.Hall, Bounds = new Rectangle(20, 115, 300, 20) };
        txtParool.Bounds = new Rectangle(20, 135, 300, 30);
        txtParool.Font = Stiil.Kiri(11);
        txtParool.UseSystemPasswordChar = true;     // näitab punkte

        btnLogin.Text = "Logi sisse";
        btnLogin.Bounds = new Rectangle(20, 185, 300, 42);
        Stiil.Nupp(btnLogin, Stiil.Punane);
        btnLogin.Click += btnLogin_Click;

        lblVeateade.Bounds = new Rectangle(20, 235, 300, 30);
        lblVeateade.ForeColor = Stiil.Punane;
        lblVeateade.TextAlign = ContentAlignment.MiddleCenter;

        kaart.Controls.AddRange(new Control[] { pealkiri, lblEmail, txtEmail, lblParool, txtParool, btnLogin, lblVeateade });

        AcceptButton = btnLogin;   // Enter-klahv vajutab nuppu
        Controls.Add(kaart);
        Controls.Add(pais);
    }

    private void btnLogin_Click(object? sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim().ToLower();

        using var db = new ToodedContext();
        var kasutaja = db.Kasutajad.FirstOrDefault(k => k.Email == email);

        // Sama teade mõlemal juhul: võõras ei saa teada, kas selline e-post on olemas
        if (kasutaja == null || !PasswordHelper.Verify(txtParool.Text, kasutaja.ParoolHash))
        {
            lblVeateade.Text = "Vale e-post või parool.";
            txtParool.Clear();
            return;
        }

        AktiivneKasutaja.Nimi = kasutaja.Nimi;
        AktiivneKasutaja.Roll = kasutaja.Roll;

        var pood = new Form3();
        pood.FormClosed += (s, a) =>
        {
            if (pood.LogiValja)
            {
                // tagasi sisselogimise aknasse
                Ostukorv.Tyhjenda();
                txtParool.Clear();
                lblVeateade.Text = "";
                Show();
            }
            else
            {
                Close();   // poe sulgemine lõpetab programmi
            }
        };
        pood.Show();
        Hide();
    }
}
