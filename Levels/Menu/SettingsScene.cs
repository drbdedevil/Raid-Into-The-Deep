using Godot;
using System;

public partial class SettingsScene : Node
{
	
	private CheckButton _fullscreenCheckButton;
	private Button _returnButton;
	private HSlider _audioVolumeSlider;
	
	
	
	public override void _Ready()
	{
		_fullscreenCheckButton = GetNode<CheckButton>("VBoxContainer/FullscreenContainer/MarginContainer/CheckButton");
		_fullscreenCheckButton.ButtonPressed = (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen);
		_fullscreenCheckButton.Toggled += SetFullscreen;
		
		_returnButton =  GetNode<Button>("EscapeButton");
		_returnButton.ButtonUp += OnReturnToMainMenu;
		
		_audioVolumeSlider = GetNode<HSlider>("VBoxContainer/MusicContainer/HSlider");
		_audioVolumeSlider.ValueChanged += OnAudioVolumeChanged;
		
		_audioVolumeSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(0));
	}

	public override void _Process(double delta)
	{
	}

	private void SetFullscreen(bool isFullscreen)
	{
		if (isFullscreen) DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
	}

	private void OnReturnToMainMenu()
	{
		GetTree().ChangeSceneToFile("res://Levels/Menu/MainMenu.tscn");
	}
	
	private void OnAudioVolumeChanged(double value)
	{
		float volumeDb = Mathf.LinearToDb((float)value);
        
		AudioServer.SetBusVolumeDb(0, volumeDb);
	}
}
