namespace Tooded_EFCore;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void btnTooted_Click(object? sender, EventArgs e)
    {
        using var vorm = new Form2();
        vorm.ShowDialog();
    }

    private void btnPood_Click(object? sender, EventArgs e)
    {
        using var vorm = new Form3();
        vorm.ShowDialog();
    }

    private void btnArve_Click(object? sender, EventArgs e)
    {
        using var vorm = new ArveForm();
        vorm.ShowDialog();
    }
}
