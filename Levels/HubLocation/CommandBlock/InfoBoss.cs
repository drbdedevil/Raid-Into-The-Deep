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

	public void SetInfos(MapNodeType bossType)
	{
		var nameLabel = GetNode<Label>("TextureRect/MarginContainer/ColorRect/HBoxContainer/VBoxContainer/NameLabel");
		var textureRect = GetNode<TextureRect>("TextureRect/MarginContainer/ColorRect/HBoxContainer/MarginContainer/TextureRect");

		switch (bossType)
		{
			case MapNodeType.SpiderBoss:
				Texture2D texture1 = GD.Load<Texture2D>("res://Textures/Characters/Bosses/SpiderBoss.png");
				textureRect.Texture = texture1;
				nameLabel.Text = "Паук, пожиратель племён";
				break;
			case MapNodeType.TankBoss:
				Texture2D texture2 = GD.Load<Texture2D>("res://Textures/Characters/Bosses/Tank.png");
				textureRect.Texture = texture2;
				nameLabel.Text = "Бронированный медленный разрушитель";
				break;
			case MapNodeType.VegetableBoss:
				Texture2D texture3 = GD.Load<Texture2D>("res://Textures/Characters/Bosses/Vegetable.png");
				textureRect.Texture = texture3;
				nameLabel.Text = "Гигантская разросшаяся мухоловка";
				break;
			default:
				break;
		}
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
