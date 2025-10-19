using Godot;
using System;

public partial class InfoBoss : Control
{
	[Export]
	public bool bIsDefeated = false;

	public override void _Ready()
	{
		Check();
	}

	public void Check()
	{
		if (bIsDefeated)
		{
			var colorRect = GetNode<ColorRect>("TextureRect/MarginContainer/ColorRect");
			colorRect.Color = Color.FromString("#8CFF9B", Colors.White);

			var nameLabel = GetNode<Label>("TextureRect/MarginContainer/ColorRect/HBoxContainer/VBoxContainer/NameLabel");
			nameLabel.AddThemeColorOverride("font_color", Colors.Black);

			var resultLabel = GetNode<Label>("TextureRect/MarginContainer/ColorRect/HBoxContainer/VBoxContainer/ResultLabel");
			resultLabel.AddThemeColorOverride("font_color", Color.FromString("#04C946", Colors.White));
			resultLabel.Text = "Побеждён";
		}
		else
		{
			var colorRect = GetNode<ColorRect>("TextureRect/MarginContainer/ColorRect");
			colorRect.Color = Color.FromString("#ff6866", Colors.White);

			var nameLabel = GetNode<Label>("TextureRect/MarginContainer/ColorRect/HBoxContainer/VBoxContainer/NameLabel");
			nameLabel.AddThemeColorOverride("font_color", Colors.White);

			var resultLabel = GetNode<Label>("TextureRect/MarginContainer/ColorRect/HBoxContainer/VBoxContainer/ResultLabel");
			resultLabel.AddThemeColorOverride("font_color", Color.FromString("#ffffff", Colors.White));
			resultLabel.Text = "Не побеждён";
		}
	}
}
