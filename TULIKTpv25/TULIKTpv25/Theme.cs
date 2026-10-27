namespace Tooded_EFCore;

// Ühtne tume teema. Theme.Apply(this) värvib vormi kõik elemendid.
public static class Theme
{
    public static readonly Color Bg = Color.FromArgb(18, 20, 28);
    public static readonly Color Panel = Color.FromArgb(27, 31, 43);
    public static readonly Color Panel2 = Color.FromArgb(40, 45, 62);
    public static readonly Color Text = Color.FromArgb(235, 238, 245);
    public static readonly Color Muted = Color.FromArgb(140, 148, 168);
    public static readonly Color Accent = Color.FromArgb(255, 106, 61);
    public static readonly Color Good = Color.FromArgb(52, 211, 153);
    public static readonly Color Bad = Color.FromArgb(248, 113, 113);

    public static void StyleButton(Button b, bool primary)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = primary ? Accent : Panel2;
        b.FlatAppearance.MouseOverBackColor = primary ? Color.FromArgb(255, 135, 95) : Color.FromArgb(56, 63, 86);
        b.ForeColor = Color.White;
        b.Cursor = Cursors.Hand;
        b.Font = new Font("Segoe UI", 10, FontStyle.Bold);
    }

    public static void Apply(Control c)
    {
        switch (c)
        {
            case Form f: f.BackColor = Bg; f.ForeColor = Text; f.Font = new Font("Segoe UI", 10); break;
            case Button b: StyleButton(b, (b.Tag as string) == "primary"); break;
            case TextBox t: t.BackColor = Panel2; t.ForeColor = Text; t.BorderStyle = BorderStyle.FixedSingle; break;
            case ComboBox cb: cb.FlatStyle = FlatStyle.Flat; cb.BackColor = Panel2; cb.ForeColor = Text; break;
            case NumericUpDown n: n.BackColor = Panel2; n.ForeColor = Text; break;
            case ListBox l: l.BackColor = Panel; l.ForeColor = Text; l.BorderStyle = BorderStyle.None; l.ItemHeight = 28; break;
            case DataGridView g: StyleGrid(g); break;
            case Label lb:
                lb.BackColor = Color.Transparent;
                if (lb.ForeColor == SystemColors.ControlText) lb.ForeColor = Text;
                break;
            case Panel p when p.BackColor == SystemColors.Control: p.BackColor = Bg; break;
        }
        foreach (Control child in c.Controls) Apply(child);
    }

    private static void StyleGrid(DataGridView g)
    {
        g.EnableHeadersVisualStyles = false;
        g.BackgroundColor = Bg;
        g.BorderStyle = BorderStyle.None;
        g.GridColor = Panel2;
        g.RowHeadersVisible = false;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeight = 34;
        g.ColumnHeadersDefaultCellStyle.BackColor = Panel2;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Text;
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Panel2;
        g.DefaultCellStyle.BackColor = Panel;
        g.DefaultCellStyle.ForeColor = Text;
        g.DefaultCellStyle.SelectionBackColor = Accent;
        g.DefaultCellStyle.SelectionForeColor = Color.White;
        g.RowTemplate.Height = 28;
    }
}
