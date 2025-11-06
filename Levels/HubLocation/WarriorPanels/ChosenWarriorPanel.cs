using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class ChosenWarriorPanel : Control
{
    private Warrior warrior = new();
    [Export]
	public PackedScene CharacterListScene;

    public override void _Ready()
    {
        // warrior = GetNode<Warrior>("WarriorPanel");

        var ListButton = GetNode<Button>("ColorRect/ColorRect/ColorRect/VBoxContainer/TextureRect/MarginContainer/Button");
        ListButton.ButtonDown += OnListButtonPressed;

        TextureButton weaponButton = GetNode<TextureButton>("ColorRect/ColorRect/ColorRect/VBoxContainer/HBoxContainer/WeaponButton");
        weaponButton.Pressed += OnWeaponButtonPressed;

        TextureButton skillButton = GetNode<TextureButton>("ColorRect/ColorRect/ColorRect/VBoxContainer/HBoxContainer/SkillButton");
        skillButton.Pressed += OnSkillButtonPressed;
    }
    private void OnListButtonPressed()
	{
		var navigator = GetTree().Root.FindChild("PopupPanel", recursive: true, owned: false) as PopupNavigator;
		CharacterList characterList = CharacterListScene.Instantiate() as CharacterList;
		characterList.warriorOwner = warrior;

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
			characterList.Parent = navigator.GetCurrent();

			navigator.PushInstance(characterList);

			var hBoxContainer = navigator.GetTree().Root.FindChild("HiddenPanel", recursive: true, owned: false) as HBoxContainer;
			hBoxContainer.Modulate = new Color(1, 1, 1, 1);
			hBoxContainer.ProcessMode = ProcessModeEnum.Always;
		}
	}

    public void SetCharacterInfos(CharacterData InCharacterData)
    {
        warrior.characterData = InCharacterData;

        TextureRect textureRect = GetNode<TextureRect>("ColorRect/ColorRect/ColorRect/VBoxContainer/TextureRect");
        textureRect.Texture = GameDataManager.Instance.charactersSpritesDatabase.CharactersSpritesArray[warrior.characterData.PortraitID];

        Label NameLabel = GetNode<Label>("ColorRect/ColorRect/ColorRect/VBoxContainer/TextureRect/VBoxContainer/NameLabel");
        NameLabel.Text = warrior.characterData.Name;

        Label LevelLabel = GetNode<Label>("ColorRect/ColorRect/ColorRect/VBoxContainer/TextureRect/VBoxContainer/HBoxContainer/LevelLabel");
        LevelLabel.Text = warrior.characterData.Level.ToString();

        PassiveSkillProgressionRow existingPassiveSkillType = GameDataManager.Instance.passiveSkillsProgressionDatabase.Progressions.FirstOrDefault(progression => progression.skillType == ESkillType.Health);
        if (existingPassiveSkillType != null)
        {
            ProgressBar progressBar = GetNode<ProgressBar>("ColorRect/ColorRect/ColorRect/VBoxContainer/MarginContainer3/ProgressBar");
            progressBar.Value = warrior.characterData.Health;

            Dictionary<string, int> passiveSkillLevels = warrior.characterData.PassiveSkillLevels;
            int healthSkillLevel = passiveSkillLevels["Здоровье"];
            int maxHealthToSet = GameDataManager.Instance.baseStatsDatabase.Health;

            for (int i = 0; i < healthSkillLevel; ++i)
            {
                maxHealthToSet += existingPassiveSkillType.increments[i];
            }

            progressBar.MaxValue = maxHealthToSet;
        }

        TextureRect weaponRect = GetNode<TextureRect>("ColorRect/ColorRect/ColorRect/VBoxContainer/HBoxContainer/WeaponButton/TextureRect");
        var existingWeapon = GameDataManager.Instance.weaponDatabase.Weapons.FirstOrDefault(weapon => weapon.Name == warrior.characterData.Weapon.Name);
        if (existingWeapon != null)
        {
            weaponRect.Texture = existingWeapon.WeaponTexture;
        }

        SkillRow skillRow = GameDataManager.Instance.activeSkillsDatabase.skillsRows.FirstOrDefault(skillRow => skillRow.skillName == warrior.characterData.ChoosenSkills);
        if (skillRow != null)
        {
            TextureRect skillRect = GetNode<TextureRect>("ColorRect/ColorRect/ColorRect/VBoxContainer/HBoxContainer/SkillButton/TextureRect");

            skillRect.Texture = skillRow.skillTextureActive;
        }
    }

    private void OnWeaponButtonPressed()
    {
        GD.Print("Персонаж " + warrior.characterData.ID + " выбрал ходить оружием");
    }
    private void OnSkillButtonPressed()
    {
        GD.Print("Персонаж " + warrior.characterData.ID + " выбрал ходить пассивным навыком");
    }
}
