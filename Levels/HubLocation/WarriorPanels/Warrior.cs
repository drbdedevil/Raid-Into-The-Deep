using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class Warrior : Node, IStackPage
{
	[Export]
	public PackedScene CharacterListScene;

	public CharacterData characterData { get; set; } = new();
	public bool bShouldHideAbilityToChange = false;

	public override void _Ready()
	{
		var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		ListButton.ButtonDown += OnListButtonPressed;
	}

	private void OnListButtonPressed()
	{
		// GD.Print("1");
		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		// GD.Print("2");
		CharacterList characterList = CharacterListScene.Instantiate() as CharacterList;
		characterList.warriorOwner = this;
		characterList.bShouldHideAbilityToChange = bShouldHideAbilityToChange;
		// GD.Print("3");

		if (!navigator.IsSomethingOpen())
		{
			navigator.PushInstance(characterList);
			navigator.Popup();

			var PanelLabel = GetTree().Root.FindChild("PanelLabel", recursive: true, owned: false) as Label;
			PanelLabel.Text = "Лист персонажа".StripEdges();

			var hBoxContainer = GetTree().Root.FindChild("HiddenPanel", recursive: true, owned: false) as HBoxContainer;
			hBoxContainer.Modulate = new Color(1, 1, 1, 0);
			hBoxContainer.ProcessMode = ProcessModeEnum.Disabled;

			characterList.Parent = characterList;
		}
		else
		{
			// GD.Print("4");
			characterList.Parent = navigator.GetCurrent();

			navigator.PushInstance(characterList);

			var hBoxContainer = navigator.GetTree().Root.FindChild("HiddenPanel", recursive: true, owned: false) as HBoxContainer;
			hBoxContainer.Modulate = new Color(1, 1, 1, 1);
			hBoxContainer.ProcessMode = ProcessModeEnum.Always;
			// GD.Print("5");
		}
		// characterList.ShowCharacterInfos();
	}

	public void SetCharacterInfos(CharacterData InCharacterData)
	{
		characterData = InCharacterData;

		TextureRect textureRect = GetNode<TextureRect>("TextureRect/HBoxContainer/MarginContainer/TextureRect");
		textureRect.Texture = GameDataManager.Instance.charactersSpritesDatabase.CharactersSpritesArray[characterData.PortraitID];

		Label NameLabel = GetNode<Label>("TextureRect/HBoxContainer/VBoxContainer/NameLabel");
		NameLabel.Text = characterData.Name;

		Label LevelLabel = GetNode<Label>("TextureRect/HBoxContainer/VBoxContainer/HBoxContainer/LevelLabel");
		LevelLabel.Text = characterData.Level.ToString();

		PassiveSkillProgressionRow existingPassiveSkillType = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progression => progression.skillType == ESkillType.Health);
		if (existingPassiveSkillType != null)
		{
			ProgressBar progressBar = GetNode<ProgressBar>("TextureRect/HBoxContainer/VBoxContainer/MarginContainer3/ProgressBar");
			progressBar.Value = characterData.Health;

			Dictionary<string, int> passiveSkillLevels = characterData.PassiveSkillLevels;
			int healthSkillLevel = passiveSkillLevels["Здоровье"];
			int maxHealthToSet = GameDataManager.Instance.baseStatsDatabase.Health;

			for (int i = 0; i < healthSkillLevel; ++i)
			{
				maxHealthToSet += existingPassiveSkillType.increments[i];
			}

			progressBar.MaxValue = maxHealthToSet;
		}
	}
	public void ReplaceCharacter()
	{
		if (GameDataManager.Instance.livingSpaceDataManager.IsCharacterInUsedList(characterData.ID))
		{
			if (GameDataManager.Instance.livingSpaceDataManager.TryAddCharacterToReserved(characterData, 1))
			{
				GameDataManager.Instance.livingSpaceDataManager.TryDeleteCharacterFromUsed(characterData.ID);
			}
		}
		else if (GameDataManager.Instance.livingSpaceDataManager.IsCharacterInReservedList(characterData.ID))
		{
			if (GameDataManager.Instance.livingSpaceDataManager.TryAddCharacterToUsed(characterData))
			{
				GameDataManager.Instance.livingSpaceDataManager.TryDeleteCharacterFromReserved(characterData.ID);
			}
			else
			{
				NotificationSystem.Instance.ShowMessage("В команде нет места для этого персонажа.", EMessageType.Alert);
			}
		}
	}
	
	public void DebugCharacterInfos()
	{
		ProgressBar progressBar = GetNode<ProgressBar>("TextureRect/HBoxContainer/VBoxContainer/MarginContainer3/ProgressBar");
		int passiveSkills = 0;
		foreach (KeyValuePair<string, int> skill in characterData.PassiveSkillLevels)
		{
			passiveSkills += skill.Value;
		}	

		GD.Print("");

		GD.Print(characterData.Name + ": " + "Уровень: " + characterData.Level.ToString());
		GD.Print("Количество пассивных навыков: " + passiveSkills.ToString());
		GD.Print("Количество активных навыков: " + characterData.ActiveSkills.Count.ToString());
		GD.Print(" -              - - - - - - - - - - - - - -                ");
		GD.Print("Здоровье: " + characterData.Health.ToString() + ", где MaxHealth = " + progressBar.MaxValue.ToString());
		GD.Print("Скорость: " + characterData.Speed.ToString());
		GD.Print("Урон: " + characterData.Damage.ToString());
		GD.Print("Урон от эффектов: " + characterData.DamageByEffect.ToString());
		GD.Print("Хил: " + characterData.Heal.ToString());
		GD.Print(" -              - - - - - - - - - - - - - -                ");
		GD.Print("Уровень навыка \"Здоровье\": " + characterData.PassiveSkillLevels["Здоровье"].ToString());
		GD.Print("Уровень навыка \"Скорость\": " + characterData.PassiveSkillLevels["Скорость"].ToString());
		GD.Print("Уровень навыка \"Урон\": " + characterData.PassiveSkillLevels["Урон"].ToString());
		GD.Print("Уровень навыка \"Урон от эффектов\": " + characterData.PassiveSkillLevels["Урон от эффектов"].ToString());
		GD.Print("Уровень навыка \"Лечение\": " + characterData.PassiveSkillLevels["Лечение"].ToString());
		GD.Print(" -              - - - - - - - - - - - - - -                ");
		GD.Print("Прокаченные активные навыки:");
		foreach (string activeSkills in characterData.ActiveSkills)
		{
			GD.Print("\t" + activeSkills);
		}

		GD.Print(" --------------------------------------------------------- ");
	}

	public void OnShow()
	{
		var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		ListButton.ReleaseFocus();
		// ListButton.ButtonDown += OnListButtonPressed;
	}
	public void OnHide()
	{
		// var ListButton = GetNode<Button>("TextureRect/HBoxContainer/MarginContainer/TextureRect/MarginContainer/Button");
		// ListButton.ButtonDown -= OnListButtonPressed;
	}
}
