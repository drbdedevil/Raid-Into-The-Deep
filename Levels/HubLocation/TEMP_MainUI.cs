using Godot;
using System;

public partial class TEMP_MainUI : Node
{
	[Export]
	private PackedScene PopupScene;

	public override void _Ready()
	{
		var button = GetNode<Button>("MainPanel/Control/OpenPopupButton");
		// button.Pressed += OnOpenPopupPressed;
		button.ButtonDown += OnOpenPopupPressed;

		var close_button = GetNode<Button>("PopupPanel/HBoxContainer/MarginContainer/ClosePopupButton");
		// close_button.Pressed += OnClosePopupPressed;
		close_button.ButtonDown += OnClosePopupPressed;
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
