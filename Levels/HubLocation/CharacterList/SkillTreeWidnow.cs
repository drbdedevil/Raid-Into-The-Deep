using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SkillTreeWidnow : ColorRect, IStackPage
{
	[Export]
	public PackedScene skillInfoPanelScene;
	[Signal]
	public delegate void RequestBackEventHandler();

	public Node Parent = null;

	public override void _Ready()
	{
		var BackButton = GetNode<TextureButton>("VBoxContainer/MarginContainer/HiddenPanel/TextureButton");
		BackButton.ButtonDown += OnBackButtonPressed;

		if (Parent is CharacterList characterList)
		{
			Label NameLabel = GetNode<Label>("VBoxContainer/MarginContainer/HiddenPanel/NameLabel");
			NameLabel.Text = characterList.warriorOwner.characterData.Name;
		}
		UpdateSkillPointsView();

		RecognizeAndBindingSkillButtons();
	}

	private void OnBackButtonPressed()
	{
		EmitSignal(SignalName.RequestBack);
	}

	private void UpdateSkillPointsView()
	{
		if (Parent is CharacterList characterList)
		{
			Label NumberLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer2/TextureRect/VBoxContainer/NumberLabel");
			NumberLabel.Text = characterList.warriorOwner.characterData.SkillPoints.ToString();

			var experienceDatas = GameDataManager.Instance.charactersExperienceLevelsDatabase.Levels;
			if (characterList.warriorOwner.characterData.Level >= experienceDatas.Count)
			{
				NumberLabel.Text = "---";

				Label TextLabel = GetNode<Label>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer2/TextureRect/VBoxContainer/Label");
				TextLabel.Text = "Макс.\n уровень";
			}
		}
	}
	private void RecognizeAndBindingSkillButtons()
	{
		CharacterList characterList = Parent as CharacterList;
		CharacterData characterData = characterList.warriorOwner.characterData;

		Dictionary<string, int> passiveSkillLevels = characterData.PassiveSkillLevels;
		HashSet<string> activeSkills = characterData.ActiveSkills;

		var textureRect = GetNode<TextureRect>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/TextureRect");
		foreach (Node child in textureRect.GetChildren())
		{
			if (child is SkillButton skillButton)
			{
				skillButton.OnSkillButtonPressed += SkillButtonPressed;
				skillButton.OnSkillButtonMouseEntered += SkillButtonMouseEntered;
				skillButton.OnSkillButtonMouseExited += SkillButtonMouseExited;

				if (SkillExtensions.IsPassive(skillButton.skillType))
				{
					SkillRow skillRow = GameDataManager.Instance.passiveSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == skillButton.skillType);
					if (skillRow != null)
					{
						if (passiveSkillLevels.ContainsKey(skillRow.skillName) && passiveSkillLevels[skillRow.skillName] > 0)
						{
							skillButton.TextureNormal = skillRow.skillTextureUpgraded;
						}
						else
						{
							skillButton.TextureNormal = skillRow.skillTextureBase;
						}

						skillButton.TextureHover = skillRow.skillTextureHover;
					}
				}
				else if (SkillExtensions.IsActive(skillButton.skillType))
				{
					SkillRow skillRow = GameDataManager.Instance.activeSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == skillButton.skillType);
					if (skillRow != null)
					{
						if (activeSkills.Contains(skillRow.skillName))
						{
							if (characterData.ChoosenSkills == skillRow.skillName)
							{
								skillButton.TextureNormal = skillRow.skillTextureActive;
							}
							else
							{
								skillButton.TextureNormal = skillRow.skillTextureUpgraded;
							}
						}
						else
						{
							skillButton.TextureNormal = skillRow.skillTextureBase;
						}

						skillButton.TextureHover = skillRow.skillTextureHover;
					}
				}
			}
		}
	}

	private void SkillButtonPressed(SkillButton skillButton)
	{
		CharacterList characterList = Parent as CharacterList;
		CharacterData characterData = characterList.warriorOwner.characterData;

		bool isMaxCharacterLevel = characterData.Level == 15;
		bool isNoSkillPoints = characterData.SkillPoints <= 0;

		if (isMaxCharacterLevel)
		{
			GD.Print("MAX LEVEL OF CHARACTER");
		}
		if (isNoSkillPoints)
		{
			GD.Print("NO SKILL POINTS");
		}

		Dictionary<string, int> passiveSkillLevels = characterData.PassiveSkillLevels;
		HashSet<string> activeSkills = characterData.ActiveSkills;

		if (SkillExtensions.IsPassive(skillButton.skillType))
		{
			SkillRow skillRow = GameDataManager.Instance.passiveSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == skillButton.skillType);
			if (skillRow != null)
			{
				PassiveSkillProgressionRow progressionRow = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progressionRow => progressionRow.skillType == skillButton.skillType);
				if (passiveSkillLevels[skillRow.skillName] < progressionRow.increments.Count && !isMaxCharacterLevel && !isNoSkillPoints)
				{
					passiveSkillLevels[skillRow.skillName] += 1;
					
					if (progressionRow != null)
					{
						switch (progressionRow.skillType)
						{
							case ESkillType.Health:
								characterData.Health += progressionRow.increments[passiveSkillLevels[skillRow.skillName] - 1];
								break;
							case ESkillType.Speed:
								characterData.Speed += progressionRow.increments[passiveSkillLevels[skillRow.skillName] - 1];
								break;
							case ESkillType.Damage:
								characterData.Damage += progressionRow.increments[passiveSkillLevels[skillRow.skillName] - 1];
								break;
							case ESkillType.DamageByEffect:
								characterData.DamageByEffect += progressionRow.increments[passiveSkillLevels[skillRow.skillName] - 1];
								break;
							case ESkillType.Heal:
								characterData.Heal += progressionRow.increments[passiveSkillLevels[skillRow.skillName] - 1];
								break;
							default:
								break;
						}
						GameDataManager.Instance.commandBlockDataManager.Promotion(characterData);
						characterData.SkillPoints -= 1;
						characterData.Level += 1;

						SoundManager.Instance.PlaySoundOnce("res://Sound/Interface/Prokachka.wav", 0.2f);
					}
				}
				else
				{
					GD.Print("Max Level of: " + skillRow.skillName);
					return;
				}

				if (passiveSkillLevels[skillRow.skillName] >= 0)
				{
					skillButton.TextureNormal = skillRow.skillTextureUpgraded;
				}
			}
		}
		else if (SkillExtensions.IsActive(skillButton.skillType))
		{
			SkillRow skillRow = GameDataManager.Instance.activeSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == skillButton.skillType);
			if (skillRow != null)
			{
				if (activeSkills.Contains(skillRow.skillName))
				{
					characterData.ChoosenSkills = skillRow.skillName;
				}
				else if (!isMaxCharacterLevel && !isNoSkillPoints)
				{
					skillButton.TextureNormal = skillRow.skillTextureUpgraded;
					activeSkills.Add(skillRow.skillName);
					GameDataManager.Instance.commandBlockDataManager.Promotion(characterData);
					characterData.SkillPoints -= 1;
					characterData.Level += 1;

					SoundManager.Instance.PlaySoundOnce("res://Sound/Interface/Prokachka.wav", 0.2f);
				}
			}
		}

		UpdateSkillPointsView();
		UpdateSkillsButtonView();

		SkillButtonMouseExited(skillButton);
		SkillButtonMouseEntered(skillButton);

		GameDataManager.Instance.livingSpaceDataManager.EmitSignal(LivingSpaceDataManager.SignalName.OnUsedCharactersListUpdate);
		GameDataManager.Instance.livingSpaceDataManager.EmitSignal(LivingSpaceDataManager.SignalName.OnReservedCharactersListUpdate);
	}
	private void SkillButtonMouseEntered(SkillButton skillButton)
	{
		CharacterList characterList = Parent as CharacterList;
		CharacterData characterData = characterList.warriorOwner.characterData;

		SkillInfoPanel skillInfoPanel = skillInfoPanelScene.Instantiate() as SkillInfoPanel;
		ColorRect colorRect = GetNode<ColorRect>("VBoxContainer/MarginContainer2/ColorRect");
		colorRect.AddChild(skillInfoPanel);
		skillInfoPanel.ShowSkillInfos(characterData, skillButton.skillType);
		skillInfoPanel.ShrinkInfoPanel();
		skillInfoPanel.AnchorBottom = 1;
		skillInfoPanel.AnchorRight = 1;
		skillInfoPanel.OffsetBottom = -10;
		skillInfoPanel.OffsetRight = -30;
		skillInfoPanel.SetAnchorsPreset(Control.LayoutPreset.BottomRight);
	} //
	private void SkillButtonMouseExited(SkillButton skillButton)
	{
		ColorRect colorRect = GetNode<ColorRect>("VBoxContainer/MarginContainer2/ColorRect");
		foreach (Node child in colorRect.GetChildren())
		{
			if (child is SkillInfoPanel skillInfoPanel)
			{
				skillInfoPanel.QueueFree();
			}
		}
	}
	private void UpdateSkillsButtonView()
	{
		CharacterList characterList = Parent as CharacterList;
		CharacterData characterData = characterList.warriorOwner.characterData;

		Dictionary<string, int> passiveSkillLevels = characterData.PassiveSkillLevels;
		HashSet<string> activeSkills = characterData.ActiveSkills;

		var textureRect = GetNode<TextureRect>("VBoxContainer/MarginContainer2/ColorRect/MarginContainer/TextureRect");
		foreach (Node child in textureRect.GetChildren())
		{
			if (child is SkillButton skillButton)
			{
				if (SkillExtensions.IsPassive(skillButton.skillType))
				{
					SkillRow skillRow = GameDataManager.Instance.passiveSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == skillButton.skillType);
					if (skillRow != null)
					{
						if (passiveSkillLevels.ContainsKey(skillRow.skillName) && passiveSkillLevels[skillRow.skillName] > 0)
						{
							skillButton.TextureNormal = skillRow.skillTextureUpgraded;
						}
					}
				}
				else if (SkillExtensions.IsActive(skillButton.skillType))
				{
					SkillRow skillRow = GameDataManager.Instance.activeSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillType == skillButton.skillType);
					if (skillRow != null)
					{
						if (activeSkills.Contains(skillRow.skillName))
						{
							if (characterData.ChoosenSkills == skillRow.skillName)
							{
								skillButton.TextureNormal = skillRow.skillTextureActive;
							}
							else
							{
								skillButton.TextureNormal = skillRow.skillTextureUpgraded;
							}
						}
					}
				}
			}
		}
	}

	public void OnShow()
	{
		GD.Print("SkillTreeWidnow Popup shown");
	}
	public void OnHide()
	{
		GD.Print("SkillTreeWidnow Popup hidden");
	}
}
