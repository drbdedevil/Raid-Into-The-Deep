using Godot;
using System;

public partial class FiredWarriorPanel : Node
{
    public override void _Ready()
    {
        var FiredButton = GetNode<Button>("PanelContainer/TextureRect/HBoxContainer/MarginContainer2/FiredWarriorButton");
        FiredButton.ButtonDown += OnFiredButtonPressed;
    }
    
    private void OnFiredButtonPressed()
	{
		GD.Print(" -- FIRED!!! --");
	}
}
