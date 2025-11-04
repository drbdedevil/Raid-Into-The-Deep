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
        GetTree().ChangeSceneToFile("res://Levels/Menu/SavesScene.tscn");
    }
    private void OnNewGameButtonPressed()
    {
        string saveName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        GameDataManager.Instance.CreateNewGame();
        GameDataManager.Instance.SetSavePath(saveName);
        GameDataManager.Instance.Save();

        GetTree().ChangeSceneToFile("res://Levels/HubLocation/HubLocation.tscn");
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
