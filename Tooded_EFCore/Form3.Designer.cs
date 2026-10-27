namespace Tooded_EFCore;

partial class Form3
{
    private System.ComponentModel.IContainer? components = null;
    private TabControl tabPood = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tabPood = new TabControl();
        SuspendLayout();
        tabPood.Dock = DockStyle.Fill;
        tabPood.Location = new Point(0, 0);
        tabPood.Name = "tabPood";
        tabPood.Size = new Size(920, 590);
        ClientSize = new Size(920, 590);
        Controls.Add(tabPood);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Form3 - Pood";
        ResumeLayout(false);
    }
}
