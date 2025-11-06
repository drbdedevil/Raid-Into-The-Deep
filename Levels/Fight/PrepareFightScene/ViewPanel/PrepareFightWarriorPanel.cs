using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using RaidIntoTheDeep.Levels.Fight;

public partial class PrepareFightWarriorPanel : Node
{
	[Export] private Label LevelLabel;
	
	private TextureRect _playerWarriorsContainer;
	
	[Signal]
	public delegate void OnWarriorPanelLeftButtonClickedEventHandler(PrepareFightWarriorPanel warriorPanel);

	public void SetPlayerEntityData(PlayerEntity playerEntity)
	{
		LevelLabel.Text = playerEntity.Level.ToString();

		TextureRect textureRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer/TextureRect");
		textureRect.Texture = GameDataManager.Instance.charactersSpritesDatabase.CharactersSpritesArray[playerEntity.PortraitID];

		Label NameLabel = GetNode<Label>("TextureRect/HBoxContainer/VBoxContainer/NameLabel");
		NameLabel.Text = playerEntity.CharacterName;

		PassiveSkillProgressionRow existingPassiveSkillType = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progression => progression.skillType == ESkillType.Health);
		if (existingPassiveSkillType != null)
		{
			ProgressBar progressBar = GetNode<ProgressBar>("TextureRect/HBoxContainer/VBoxContainer/MarginContainer3/ProgressBar");
			progressBar.Value = playerEntity.Health;

			Dictionary<string, int> passiveSkillLevels = playerEntity.PassiveSkillLevels;
			int healthSkillLevel = passiveSkillLevels["Здоровье"];
			int maxHealthToSet = GameDataManager.Instance.baseStatsDatabase.Health;

			for (int i = 0; i < healthSkillLevel; ++i)
			{
				maxHealthToSet += existingPassiveSkillType.increments[i];
			}

			progressBar.MaxValue = maxHealthToSet;
		}
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
