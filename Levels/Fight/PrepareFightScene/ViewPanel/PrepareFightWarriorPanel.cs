using Godot;
using System;

public partial class PrepareFightWarriorPanel : Node
{
	[Export] private Label LevelLabel;
	
	private TextureRect _playerWarriorsContainer;
	
	[Signal]
	public delegate void OnWarriorPanelLeftButtonClickedEventHandler(PrepareFightWarriorPanel warriorPanel);

	public void SetCharacterData(CharacterData characterData)
	{
		LevelLabel.Text = characterData.ID;
	}
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
			if (mouseEvent.Pressed)
			{
				if (mouseEvent.ButtonIndex == MouseButton.Left) EmitSignalOnWarriorPanelLeftButtonClicked(this);
				else if (mouseEvent.ButtonIndex == MouseButton.Right) EmitSignalOnWarriorPanelLeftButtonClicked(this);
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
