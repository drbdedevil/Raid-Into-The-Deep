using Godot;
using System;

public partial class PrepareFightWarriorPanel : Node
{
	private TextureRect _playerWarriorsContainer;
	
	[Signal]
	public delegate void OnWarriorPanelClickedEventHandler(PrepareFightWarriorPanel warriorPanel);
	
	public override void _Ready()
	{
		_playerWarriorsContainer = GetNode<TextureRect>("TextureRect");
		_playerWarriorsContainer.MouseEntered += OnMouseEntered;
		_playerWarriorsContainer.MouseExited +=  OnMouseExited;
		_playerWarriorsContainer.GuiInput += OnGuiInput;
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				EmitSignalOnWarriorPanelClicked(this);
			}
		}
	}

	private void OnMouseEntered()
	{
		_playerWarriorsContainer.Modulate = new Color(1, 0, 1);
	}
	private void OnMouseExited()
	{
		_playerWarriorsContainer.Modulate = new Color(1, 1, 1);
	}
}
