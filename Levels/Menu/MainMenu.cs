using Godot;
using System;

public partial class MainMenu : Node
{
    public override void _Ready()
    {
        var button = GetNode<Button>("VBoxContainer/ContinueButton");
        button.ButtonDown += OnContinueButtonPressed;

        var button2 = GetNode<Button>("VBoxContainer/NewGameButton");
        button2.ButtonDown += OnNewGameButtonPressed;

        var button3 = GetNode<Button>("VBoxContainer/SettingsButton");
        button3.ButtonDown += OnSettingsButtonPressed;

        var button4 = GetNode<Button>("VBoxContainer/EscapeButton");
        button4.ButtonDown += OnEscapeButtonPressed;
    }

    private void OnContinueButtonPressed()
    {
        GD.Print(" -- ContinueButtonPressed --");

        GetTree().ChangeSceneToFile("res://Levels/HubLocation/HubLocation.tscn");
    }
    private void OnNewGameButtonPressed()
    {
        GD.Print(" -- NewGameButtonPressed --");
    }
    private void OnSettingsButtonPressed()
    {
        GD.Print(" -- SettingsButtonPressed --");
    }
    private void OnEscapeButtonPressed()
    {
        GD.Print(" -- EscapeButtonPressed --");

        GetTree().Quit();
	}
}
