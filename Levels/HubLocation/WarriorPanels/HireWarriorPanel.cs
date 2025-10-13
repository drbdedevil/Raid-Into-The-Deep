using Godot;
using System;

public partial class HireWarriorPanel : Node
{
	public override void _Ready()
	{
		var HireButton = GetNode<TextureButton>("PanelContainer/MarginContainer2/TextureButton");
		HireButton.ButtonDown += OnHireButtonPressed;
	}
	
	private void OnHireButtonPressed()
	{
		GD.Print(" -- HIRE!!! --");
	}
}
