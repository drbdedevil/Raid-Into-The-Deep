using Godot;
using System;

public partial class HelperLink : RichTextLabel
{
    [Export] public string PDFPath = "res://manual.pdf";
    
    public override void _Ready()
    {
        BbcodeEnabled = true;
        Text = $"[url=open_pdf] Открыть PDF руководство по игре Raid-Into-The-Deep[/url]";
        MetaClicked += OnMetaClicked;
    }
    
    private void OnMetaClicked(Variant meta)
    {
        string from = "res://manual.pdf";
        string to = "user://GameManual.pdf";

        if (!FileAccess.FileExists(to))
        {
            using var infile = FileAccess.Open(from, FileAccess.ModeFlags.Read);
            if (infile == null)
            {
                GD.PrintErr("Не удалось открыть PDF в res://");
                return;
            }

            using var outfile = FileAccess.Open(to, FileAccess.ModeFlags.Write);
            outfile.StoreBuffer(infile.GetBuffer((long)infile.GetLength()));
        }

        string abs = ProjectSettings.GlobalizePath(to);
        OS.ShellOpen(abs);
    }
}