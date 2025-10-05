using Godot;
using System;

public partial class HubLocation : Node
{
	[Export]
	private PackedScene PopupScene;

	public override void _Ready()
	{
		var MenuButton = GetNode<Button>("MainPanel/Panel/VBoxContainer/HBoxContainer/MenuButton");
		MenuButton.ButtonDown += OnMenuButtonPressed;

		var SettingsButton = GetNode<Button>("MainPanel/Panel/VBoxContainer/HBoxContainer/SettingsButton");
		SettingsButton.ButtonDown += OnSettingsButtonPressed;

		var button = GetNode<Button>("MainPanel/Control/OpenPopupButton");
		// button.Pressed += OnOpenPopupPressed;
		button.ButtonDown += OnOpenPopupPressed;

		var close_button = GetNode<Button>("PopupPanel/HBoxContainer/MarginContainer/ClosePopupButton");
		// close_button.Pressed += OnClosePopupPressed;
		close_button.ButtonDown += OnClosePopupPressed;
	}

	private void OnMenuButtonPressed()
	{
		GD.Print(" -- MenuButtonButtonPressed --");

		GetTree().ChangeSceneToFile("res://Levels/Menu/MainMenu.tscn");
	}
	private void OnSettingsButtonPressed()
	{
		GD.Print(" -- SettingsButtonPressed --");
	}

	private void OnOpenPopupPressed()
	{
		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Popup();
	}

	private void OnClosePopupPressed()
	{
		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Hide();
	}
}
