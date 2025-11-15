using Godot;
using System;

public partial class SettingsScene : Node
{
	
	private CheckButton _fullscreenCheckButton;
	private Button _returnButton;
	private HSlider _audioVolumeSlider;
	private HSlider _enemyTurnSpeedSlider;
	
	
	
	public override void _Ready()
	{
		_fullscreenCheckButton = GetNode<CheckButton>("VBoxContainer/FullscreenContainer/MarginContainer/CheckButton");
		_fullscreenCheckButton.ButtonPressed = (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen);
		_fullscreenCheckButton.Toggled += SetFullscreen;
		
		_returnButton =  GetNode<Button>("EscapeButton");
		_returnButton.ButtonUp += OnReturnToMainMenu;
		
		_audioVolumeSlider = GetNode<HSlider>("VBoxContainer/MusicContainer/HSlider");
		_audioVolumeSlider.Value = Mathf.DbToLinear(GameDataManager.Instance.SettingsData.AudioVolume);
		_audioVolumeSlider.ValueChanged += OnAudioVolumeChanged;
		
		_enemyTurnSpeedSlider =  GetNode<HSlider>("VBoxContainer/EnemyTurnSpeedContainer/HSlider");
		_enemyTurnSpeedSlider.Value = GameDataManager.Instance.SettingsData.EnemyTurnSpeed;
		_enemyTurnSpeedSlider.ValueChanged += OnEnemyTurnSpeedChanged;
		
		for (int i = 0; i < 10; i++)
		{
			GD.Print(Mathf.LinearToDb(i));
		} 
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
		GameDataManager.Instance.SaveSettings();
	}
	
	private void OnAudioVolumeChanged(double value)
	{
		float volumeDb = Mathf.LinearToDb((float)value);
		GD.Print(volumeDb);
	
		AudioServer.SetBusVolumeDb(0, volumeDb);
		GameDataManager.Instance.SettingsData.AudioVolume = volumeDb;
	}

	private void OnEnemyTurnSpeedChanged(double value)
	{
		GameDataManager.Instance.SettingsData.EnemyTurnSpeed = (int)value;
	}
}
