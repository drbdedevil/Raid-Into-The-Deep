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

		var TrainingPitsButton = GetNode<Button>("MainPanel/Control/TrainingPitsButton");
		TrainingPitsButton.ButtonDown += OnTrainingPitsPressed;

		var button = GetNode<Button>("MainPanel/Control/OpenPopupButton");
		// button.Pressed += OnOpenPopupPressed;
		button.ButtonDown += OnOpenPopupPressed;

		var close_button = GetNode<Button>("PopupPanel/MarginContainer/VBoxContainer/HBoxContainer2/MarginContainer/ClosePopupButton");
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

	private void OnTrainingPitsPressed()
	{
		var TrainingPits = GetNode<BaseInterest>("MainPanel/Control/TrainingPitsButton");
		Node sceneInstance = TrainingPits.SceneToOpen.Instantiate();

		ClearAndAddNewSceneToPopup(sceneInstance);

		var popup = GetNode<PopupPanel>("PopupPanel");
		popup.Popup();
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

	private void ClearAndAddNewSceneToPopup(Node sceneInstance)
	{
		var container = GetNode<Control>("PopupPanel/MarginContainer/VBoxContainer/SceneContainer");
		foreach (Node child in container.GetChildren())
		{
			child.QueueFree();
		}
		container.AddChild(sceneInstance);
	}
}
