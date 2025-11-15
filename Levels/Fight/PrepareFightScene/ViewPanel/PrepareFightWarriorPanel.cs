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

		TextureRect weaponRect = GetNode<TextureRect>("TextureRect/HBoxContainer2/WeaponButton/TextureRect");
		var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == playerEntity.Weapon.weaponData.Name);
		if (existingWeapon != null)
		{
			weaponRect.Texture = existingWeapon.WeaponTexture;

			TextureRect effectRect = GetNode<TextureRect>("TextureRect/HBoxContainer2/WeaponButton/TextureRect2/MarginContainer/EffectTexture");
			var existingEffect = GameDataManager.Instance.effectDatabase.Effects[playerEntity.Weapon.weaponData.EffectID];
			if (existingEffect != null)
			{
				effectRect.Texture = existingEffect.texture2D;
			}
		}
		else
		{
			Texture2D texture2D = GD.Load<Texture2D>("res://Textures/ChooseAttack/ChooseAttack_No.png");
			weaponRect.Texture = texture2D;
		}

		TextureRect skillRect = GetNode<TextureRect>("TextureRect/HBoxContainer2/SkillButton/TextureRect");
		if (playerEntity.activeSkill == null || playerEntity.activeSkill != null && playerEntity.activeSkill.skillType == ESkillType.NONE)
        {
            Texture2D texture2D = GD.Load<Texture2D>("res://Textures/ChooseAttack/ChooseAttack_No.png");
			skillRect.Texture = texture2D;
			return;
        }

		SkillRow skillRow = GameDataManager.Instance.activeSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == playerEntity.activeSkill.skillType);
		if (skillRow != null)
		{
			skillRect.Texture = skillRow.skillTextureActive;
		}
		else
		{
			Texture2D texture2D = GD.Load<Texture2D>("res://Textures/ChooseAttack/ChooseAttack_No.png");
			skillRect.Texture = texture2D;
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
