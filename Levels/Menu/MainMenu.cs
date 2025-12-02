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

		SoundManager.Instance.RemoveAllSounds();
		SoundManager.Instance.PlaySoundLoop("res://Sound/Music/FantasyMainMenu.mp3", 0.1f);
		
	}
	public override void _ExitTree()
	{
		
	}

	private void OnContinueButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Levels/Menu/SavesScene.tscn");
	}
	private void OnNewGameButtonPressed()
	{
		DifficultySelection difficultySelection = GetNode<DifficultySelection>("DifficultySelection");
		difficultySelection.ShowPopup();
	}
	private void OnSettingsButtonPressed()
	{
		GD.Print(" -- SettingsButtonPressed --");
		
		GetTree().ChangeSceneToFile("res://Levels/Menu/Settings.tscn");
	}
	private void OnEscapeButtonPressed()
	{
		GD.Print(" -- EscapeButtonPressed --");

		// SoundManager.Instance.PlaySoundOnce("res://Sound/Interface/ExitGame.wav");
		GetTree().Quit();
	}
}
